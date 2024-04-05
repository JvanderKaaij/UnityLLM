using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//<summary>
// ConnectFourLogic class is responsible for the game logic of Connect Four.
// It's important to realise that outside of the class the X is the horizontal axis and Y is the vertical axis.
// Inside this class it's the other way around. As such, the board is represented as a list of lists where the first list is the vertical axis and the second list is the horizontal axis.
//</summary>

public class ConnectFourLogic
{
    private List<List<int>> board;
    
    private int height = 6;
    private int width = 7;
    
    public int Height => height;
    public int Width => width;
    
    public List<List<int>> Board => board;
    
    public ConnectFourLogic()
    {
        board = new List<List<int>>();
    }

    public void InitBoard(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new List<List<int>>();
        for (int y = 0; y< height; y++)
        {
            board.Add(new List<int>());
            for (int x = 0; x < width; x++)
            {
                board[y].Add(0);
            }
        }
    }
    
    public void SetBoard(List<List<int>> board)
    {
        this.board = board;
    }
    
    public bool CheckState(Vector2Int pos, int player)
    {
        bool vertMatch = HasVerticalMatch(pos, player);
        bool horiMatch = HasHorizontalMatch(pos, player);
        bool diagMatch = HasDiagonalMatch(pos, player);
        
        return vertMatch || horiMatch || diagMatch;
    }
    
    public int CountEmpty()
    {
        int count = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (board[y][x] == 0) count++;
            }
        }
        return count;
    }
    
    public bool CheckDraw()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
               if(board[y][x] == 0) return false; //empty space found, game is not a draw
            }
        }
        return true; //no empty space found, game a draw
    }
    
    public bool HasVerticalMatch(Vector2Int pos, int player)
    {
        for (int i = pos.y; i < pos.y+4; i++)
        {
            if (i >= height) return false; // piece is outside the board
            if (board[i][pos.x] != player) return false; // piece is not the same as the player
        }
        return true; //all 4 pieces are the same
    }

    //check horizontal both left and right
    public bool HasHorizontalMatch(Vector2Int pos, int player)
    {
        int horizontalCount = 1;
        List<int> directions = new List<int>{1, -1};
        foreach (var dir in directions)
        {
            for (int i = 1; i < 4; i++)
            {
                int nPosX = pos.x + (i * dir);
                if (nPosX >= width || nPosX < 0){
                    continue; // piece is outside the board
                }
                
                if (board[pos.y][nPosX] == player) // piece is the same as the player
                {
                    horizontalCount++;
                    if (horizontalCount == 4) return true;
                }else
                {
                    break; // piece is not the same as the player
                }
            }
        }
        if (horizontalCount >= 4) return true;
        return false;
    }
    
    //check horizontal both left and right
    public bool HasDiagonalMatch(Vector2Int pos, int player)
    {
        int diagonalCount = 1;
        List<Vector2Int> directions = new List<Vector2Int>{new(-1, -1), new (1, 1), new (-1, 1), new (1, -1)};
        
        foreach (var dir in directions)
        {
            for (int i = 1; i < 4; i++)
            {
                int nPosX = pos.x + i * dir.x;
                int nPosY = pos.y + i * dir.y;
                if (nPosX >= width || nPosX < 0 || nPosY >= height || nPosY < 0) continue; // piece is outside the board
                if (board[nPosY][nPosX] == player)
                {
                    diagonalCount++;
                    if (diagonalCount == 4) return true;
                }
                else
                {
                    break;
                }
            }
        }
        if (diagonalCount >= 4) return true;
        return false;
    }
    
    //<summary>
    // Make a move in the board. Find the last row that is not empty and place the piece one above it.
    // Player should be -1 or 1
    // Returns Vector2Int of the position of where the piece came to rest. Null if the column is full.
    //</summary>
    public Vector2Int? DropPiece(int x, int player)
    {
        for (int y = 0; y < height; y++)
        {
            if (board[y][x] == 0) continue; // if the cell is empty
            if(y == 0) return null; // if the column is full
            board[y-1][x] = player; // place stone above
            return new Vector2Int(x, y-1);
        }
        //else it is the last row
        board[height-1][x] = player;
        return new Vector2Int(x,height-1);
    }
    
    public string BoardAsString()
    {
        string result = "";

        // Assuming 'board' is not jagged and is square/rectangular for simplicity
        if (board.Count == 0) return result; // Early return if board is empty

        // Determine the dimensions of the board
        int height = board.Count; // Vertical
        int width = board[0].Count; // Horizontal

        // Iterate through each 'column' first (to effectively rotate the board)
        for (int y = 0; y < height; y++)
        {
            result += "[ ";
            for (int x = 0; x < width; x++)
            {
                // Append the integer followed by a space
                result += board[y][x].ToString() + " ";
            }
            result += "]\n";
        }

        return result;
    }
    
    public List<float> GetBoardAsFloats()
    {
        List<float> result = new List<float>();
        for (int y = 0; y < board.Count; y++)
        {
            for (int x = 0; x < board[y].Count; x++)
            {
                result.Add((float)board[y][x]);
            }
        }
        return result;
    }
}
