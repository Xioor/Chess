using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{   
    protected Vector2Int m_PlayerPos;
    protected int m_PlayerOrientation; // determines chess forward vector

    protected ChessBoard chessBoard = ChessBoard.getInstance();

    public List<Vector2Int> m_MovementOffsets;
    public virtual List<Vector2Int> getAvailableMoves()
    {
         List<Vector2Int> availMoves = new List<Vector2Int>();
         
         foreach(Vector2Int squareToCheck in m_MovementOffsets)
        {
            if(chessBoard.isSquareAvailable(squareToCheck, this))
            {
                availMoves.Add(squareToCheck);
            }
        }        
        return availMoves;
    }

    public virtual void movePiece(Vector2Int newPos)
    {
        //Check to see if another piece is already on the square in question. 

        //Move piece to new location.

    }

    public virtual bool KillPiece()
    {
        //Play attack animation 

        return false; 
    }

    public int getOrientation()
    {
        return m_PlayerOrientation;
    }
}
