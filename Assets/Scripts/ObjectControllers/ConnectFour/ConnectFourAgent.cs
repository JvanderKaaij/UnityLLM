using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ConnectFourAgent:Agent
{
    [SerializeField] private ConnectFourBoard board;
    [SerializeField] public int player;
    public bool madeMove = false;
    
    List<int> maskedMoves = new();
    
    public ConnectFourLogic Logic => board.logic;
    
    public override void OnEpisodeBegin()
    {
        maskedMoves = new();
    }
    
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        foreach (var masked in maskedMoves)
        {
            actionMask.SetActionEnabled(0, masked, false);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(board.logic.GetBoardAsFloats());
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        DoMove(actionBuffers.DiscreteActions[0]);
    }
    
    //<summary>
    // Do a move, and keep trying until a valid move is done
    //</summary>
    public void DoMove(int pos)
    {
        madeMove = false;
        Vector2Int? position = board.logic.DropPiece(pos, player);
        bool won;
        if (position.HasValue)
        {
            madeMove = true;
            won = board.logic.CheckState(position.Value, player);
            if (won) board.EndGame(this);
        }
        else
        {
            maskedMoves.Add(pos);
            Debug.Log($"Invalid Move {pos}, request new decision");
            Debug.Log(board.logic.BoardAsString());
            madeMove = false;
            //RequestDecision(); //invalid move - request new move
        }
    }
    public void RewardWin()
    {
        SetReward(1f);
        EndEpisode();
    }
    
    public void RewardLose()
    {
        SetReward(-1f);
        EndEpisode();
    }

    public void RewardDraw()
    {
        SetReward(0f);
        EndEpisode();
    }
}
