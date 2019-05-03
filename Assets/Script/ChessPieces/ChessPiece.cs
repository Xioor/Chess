using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public bool m_Moveable;   
    protected Vector2Int m_PlayerPos;
    public Vector2Int m_StartPos;
    protected int m_PlayerOrientation = 1; // determines chess forward vector

    protected PieceType m_Type;

    protected ChessBoard chessBoard = ChessBoard.getInstance();

    public List<Vector2Int> m_MovementOffsets;
    public List<Vector2Int> m_DirectionVectors;

    public virtual void SetStartInfo(Vector2Int intialPos, int orientaion)
    {
        float posX = intialPos.x * chessBoard.m_SquareSize.x + chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = intialPos.y * chessBoard.m_SquareSize.y + chessBoard.m_BoardStartPos.transform.position.z;
        
        transform.position.Set(posX, transform.position.y, posZ);
        m_PlayerOrientation = orientaion;

    }

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

        //Check Squares in direction
        foreach(Vector2Int direction in m_DirectionVectors)
        {
            int i = 1;
            bool checkDirection = true;
            while(checkDirection)
            {
                Vector2Int squareToCheck = direction * i + this.m_PlayerPos;
                checkDirection = chessBoard.isSquareAvailable(squareToCheck, this);

                if(!checkDirection) {break;}

                availMoves.Add(squareToCheck);
                i++;
            }
        }

        return availMoves;
    }

    //TODO: 
    public virtual void movePiece(Vector2Int newPos)
    {
        //Check to see if another piece is already on the square in question. 
        //Move piece to new location.
        float posX = newPos.x * chessBoard.m_SquareSize.x + chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = newPos.y * chessBoard.m_SquareSize.y + chessBoard.m_BoardStartPos.transform.position.z;
        
        transform.position = new Vector3(posX, transform.position.y, posZ);
        m_PlayerPos = newPos;
    }

    //TODO: added logic to remove piece and update board.
    public virtual bool KillPiece()
    {
        //Play attack animation 
        return false; 
    }

    public int getOrientation()
    {
        return m_PlayerOrientation;
    }

    public Vector2Int getStartPos()
    {
        return m_StartPos;
    }

     public Vector2Int getCurrentPos()
    {
        return m_PlayerPos;
    }

    public virtual PieceType getPieceType()
    {
        return m_Type;
    }

    public virtual void TryAddingAvailableMove(ref List<Vector2Int> availableMoves, int x, int y, bool bAvaiableOnlyIfKillPossible)
    {
        Vector2Int newMove = new Vector2Int(m_PlayerPos.x + x * m_PlayerOrientation, m_PlayerPos.y + y * m_PlayerOrientation);
        if (chessBoard.isSquareAvailable(newMove, this, bAvaiableOnlyIfKillPossible))
        {
            availableMoves.Add(newMove);
        }
    }
}
