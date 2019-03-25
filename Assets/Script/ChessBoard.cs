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
    
    private ChessBoard (){}
    
    // Start is called before the first frame update
    Vector3 squareLocations;
    Square[,] squares = new Square[8,8];

    public bool isSquareAvailable(Vector2Int square, ChessPiece pieceToMove)
    {
        //Check if square is in bounds.
        if(square.x > 7 || square.x < 0 || square.y > 7 || square.y < 0)
            return false; //Out of bounds

        //check to see if square is ocupied
        if(squares[square.x, square.y].isOccupied)
        {
            //Check if the occupying piece has same orientation. 
            if(squares[square.x, square.y].piece.getOrientation() == pieceToMove.getOrientation())
                //occupied by an enemy. 
                return true;            
        }
        
        //does not fail, so return true;
        return true;
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
