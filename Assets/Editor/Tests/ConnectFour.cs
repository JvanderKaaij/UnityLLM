using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ConnectFour
{
    private ConnectFourLogic connectFourLogic;
    //setup test
    [SetUp]
    public void Setup()
    {
        connectFourLogic = new ConnectFourLogic();
        connectFourLogic.InitBoard(7, 6);
    }

    [Test]
    public void VerticalMatch()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsTrue(connectFourLogic.HasVerticalMatch(new Vector2Int(3, 2), -1));
    }
    
    [Test]
    public void VerticalMatchNotEnough()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsFalse(connectFourLogic.HasVerticalMatch(new Vector2Int(3, 3), -1));
    }
    
    [Test]
    public void VerticalMatchOtherPlayer()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,1,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsFalse(connectFourLogic.HasVerticalMatch(new Vector2Int(3, 2), 1));
    }

    [Test]
    public void HorizontalMatch()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,-1,-1,-1,-1,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsTrue(connectFourLogic.HasHorizontalMatch(new Vector2Int(1, 5), -1));
        Assert.IsTrue(connectFourLogic.HasHorizontalMatch(new Vector2Int(4, 5), -1));
    }
    
    [Test]
    public void HorizontalMatchOnRow()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{1,1,1,1,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsTrue(connectFourLogic.HasHorizontalMatch(new Vector2Int(2, 2), 1));
    }
    
    [Test]
    public void HorizontalMatchNotEnough()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{1,1,1,-1,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsFalse(connectFourLogic.HasHorizontalMatch(new Vector2Int(2, 2), 1));
    }
    
    [Test]
    public void HorizontalMatchBroken()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{1,1,-1,1,1,1,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsFalse(connectFourLogic.HasHorizontalMatch(new Vector2Int(3, 2), 1));
    }
    
    [Test]
    public void DiagonalMatch()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,-1,0,0,0,0});
        board.Add(new List<int>{0,-1,0,0,0,0,0});
        board.Add(new List<int>{-1,0,0,0,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsTrue(connectFourLogic.HasDiagonalMatch(new Vector2Int(2, 3), -1));
        Assert.IsTrue(connectFourLogic.HasDiagonalMatch(new Vector2Int(0, 5), -1));
    }
    
    [Test]
    public void DiagonalMatchBroken()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,1,0,0,0,0});
        board.Add(new List<int>{0,-1,0,0,0,0,0});
        board.Add(new List<int>{-1,0,0,0,0,0,0});
        connectFourLogic.SetBoard(board);
        Assert.IsFalse(connectFourLogic.HasDiagonalMatch(new Vector2Int(1, 4), -1));
    }
    
    [Test]
    public void DiagonalMatchInverse()
    {
        connectFourLogic.InitBoard(7, 6);
        List<List<int>> board = new List<List<int>>();
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,0,0,0,0});
        board.Add(new List<int>{0,0,0,-1,0,0,0});
        board.Add(new List<int>{0,0,0,0,-1,0,0});
        board.Add(new List<int>{0,0,0,0,0,-1,0});
        board.Add(new List<int>{0,0,0,0,0,0,-1});
        connectFourLogic.SetBoard(board);
        Assert.IsTrue(connectFourLogic.HasDiagonalMatch(new Vector2Int(4, 3), -1));
    }

    [Test]
    public void MakeMovePlayerOne()
    {
        connectFourLogic.InitBoard(7, 6);
        Vector2Int? pos1 = connectFourLogic.DropPiece(0, 1);
        Assert.IsTrue(pos1 == new Vector2Int(0, 5));
        Vector2Int? pos2 = connectFourLogic.DropPiece(0, 1);
        Assert.IsTrue(pos2 == new Vector2Int(0, 4));
        Vector2Int? pos3 = connectFourLogic.DropPiece(0, 1);
        Vector2Int? pos4 = connectFourLogic.DropPiece(0, 1);
        Vector2Int? pos5 = connectFourLogic.DropPiece(0, 1);
        Vector2Int? pos6 = connectFourLogic.DropPiece(0, 1);
        Vector2Int? pos7 = connectFourLogic.DropPiece(0, 1); //should be full and return null
        Assert.IsFalse(pos7.HasValue);
    }
    
    [Test]
    public void MakeMovePlayerOneWins()
    {
        connectFourLogic.InitBoard(7, 6);
        connectFourLogic.DropPiece(0, -1);
        connectFourLogic.DropPiece(0, 1);
        connectFourLogic.DropPiece(1, -1);
        connectFourLogic.DropPiece(2, -1);
        Vector2Int? lastMove1 = connectFourLogic.DropPiece(3, -1);
        Assert.IsTrue(connectFourLogic.HasHorizontalMatch(lastMove1.Value, -1));
    }
    
    [Test]
    public void DropPiece()
    {
        Vector2Int? state;
        connectFourLogic.InitBoard(7, 6);
        connectFourLogic.DropPiece(0, -1);
        Assert.IsTrue(connectFourLogic.Board[5][0] == -1);
        
        connectFourLogic.DropPiece(5, 1);
        Assert.IsTrue(connectFourLogic.Board[5][5] == 1);
        
        connectFourLogic.DropPiece(5, -1);
        Assert.IsTrue(connectFourLogic.Board[4][5] == -1);
        
        Debug.Log(connectFourLogic.BoardAsString());
        
    }
    
    [Test]
    public void TestGame()
    {
        Vector2Int? state;
        connectFourLogic.InitBoard(7, 6);
        connectFourLogic.DropPiece(0, -1);
        connectFourLogic.DropPiece(5, 1);
        connectFourLogic.DropPiece(1, -1);
        
        state = connectFourLogic.DropPiece(2, 1);
        Assert.IsFalse(connectFourLogic.CheckState(state.Value, -1));
        
        state = connectFourLogic.DropPiece(2, 1);
        Assert.IsFalse(connectFourLogic.CheckState(state.Value, -1));
        
        Debug.Log(connectFourLogic.BoardAsString());
        
    }
    
}
