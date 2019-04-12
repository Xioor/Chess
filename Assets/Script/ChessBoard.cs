/*

This class is a singleton that holds all of the info for the board. 
This will hold:
    Piece location. 

 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{  
    static ChessBoard m_Instace = null;

    public static ChessBoard getInstance()
    {
        if(m_Instace == null)
        {
            m_Instace = new ChessBoard();
        }
        return m_Instace;
    }
    
    private ChessBoard ()
    {
        generateNewBoard();
    }
    
    // Start is called before the first frame update
    Vector3 squareLocations;
    Square[,] squares = new Square[8,8];

    public bool isSquareAvailable(Vector2Int square, ChessPiece pieceToMove, bool bAvaiableOnlyIfKillPossible = false)
    {
        //Check if square is in bounds.
        if(square.x > 7 || square.x < 0 || square.y > 7 || square.y < 0)
            return false; //Out of bounds

        Square squareDestination = squares[square.x, square.y];

        if (squareDestination.isOccupied && squareDestination.piece.getOrientation() != pieceToMove.getOrientation())
        {
            return true;
        }
        else if(!squareDestination.isOccupied && !bAvaiableOnlyIfKillPossible)
        {
            return true; 
        }

        return false;
    }

    public void generateNewBoard()
    {
        //TODO: add logic to generate new board.
    }

}

struct Square
{
    Vector2Int gridLocation;
    public ChessPiece piece;    
    public bool isOccupied;

    void setPiece(ChessPiece piece)
    {
        isOccupied = true;
        this.piece = piece;
    }

    void removePiece()
    {
        isOccupied = false;
        piece = null;
    }
}
