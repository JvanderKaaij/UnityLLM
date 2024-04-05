using UnityEngine;

public class ConnectFourBoard : MonoBehaviour
{
    [SerializeField] ConnectFourView view;

    [SerializeField] private ConnectFourAgent playerOne;
    [SerializeField] private ConnectFourAgent playerTwo;
    public ConnectFourLogic logic = new();

    private ConnectFourAgent currentPlayer;

    private bool playing;

    public bool training = false;
    
    private void Start()
    {
        if(training) Reset();
    }

    public void Update()
    {
        if (!playing || !training) return;
        
        currentPlayer.RequestDecision();
        
        int spacesLeft = logic.CountEmpty();
        Debug.Log($"Empty Spaces left: {spacesLeft}");
        
        if(spacesLeft == 0){
            Debug.Log($"[Game Over] Draw");
            playerOne.RewardDraw();
            playerTwo.RewardDraw();
            Reset();
            return;
        }
        
        if (currentPlayer.madeMove)
        {
            currentPlayer.madeMove = false;
            SwitchPlayer();
        }
    }

    public void HumanMakeMove(int pos)
    {
        Vector2Int? humanMove = logic.DropPiece(pos, -1);
        if (humanMove.HasValue)
        {
            logic.CheckState(humanMove.Value, -1);
            Debug.Log(logic.BoardAsString());
            playerOne.RequestDecision();
        }
        else
        {
            Debug.LogWarning("Illegal move");
        }
    }

    public void Reset()
    {
        view.Reset();
        logic.InitBoard(7, 6);
        currentPlayer = playerOne;
        playing = true;
    }
    
    public void EndGame(ConnectFourAgent winner)
    {
        
        Debug.Log($"[Game Over] Winner: {winner.player} ");
        Debug.Log(logic.BoardAsString());
        
        winner.RewardWin();
        Other(winner).RewardLose();
        Reset();
    }
    
    private ConnectFourAgent Other(ConnectFourAgent player)
    {
        return player == playerOne ? playerTwo : playerOne;
    }
    
    private void SwitchPlayer()
    {
        currentPlayer = currentPlayer == playerOne ? playerTwo : playerOne;
    }
    
}
