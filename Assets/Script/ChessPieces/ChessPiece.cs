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

    protected ChessBoard m_chessBoard = ChessBoard.getInstance();

    public List<Vector2Int> m_MovementOffsets;
    public List<Vector2Int> m_DirectionVectors;

    public virtual void SetStartInfo(Vector2Int intialPos, int orientaion)
    {
        m_StartPos = intialPos;
        m_PlayerPos = intialPos;
        m_PlayerOrientation = orientaion;

        float posX = intialPos.x * m_chessBoard.m_SquareSize.x + m_chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = intialPos.y * m_chessBoard.m_SquareSize.y + m_chessBoard.m_BoardStartPos.transform.position.z;

        transform.position = new Vector3(posX, transform.position.y, posZ);
        m_Moveable = true;
    }

    public virtual List<Vector2Int> getAvailableMoves(List<Vector2Int> squaresOverride = default(List<Vector2Int>), PieceMoveRestriction pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, bool bSameColorOverride = false, bool bFirstCheck = false)
    {
        List<Vector2Int> availMoves = new List<Vector2Int>();

        foreach(Vector2Int squareToCheck in m_MovementOffsets)
        {
            if(m_chessBoard.isSquareAvailable(squareToCheck + this.m_PlayerPos, this, pieceMoveRestriction, bSameColorOverride))
            {
                availMoves.Add(squareToCheck);
            }

            if (squaresOverride != null && squaresOverride.Count > 0)
            {
                foreach (Vector2Int squareOveride in squaresOverride)
                {
                    if (squareOveride == squareToCheck)
                    {
                        availMoves.Add(squareOveride);
                    }
                }
            }
        }  

        //Check Squares in direction
        foreach(Vector2Int direction in m_DirectionVectors)
        {
            int i = 1;
            bool checkDirection = true;
            bool bIgnoreFirstKingCheck = true;
            while(checkDirection)
            {
                Vector2Int squareToCheck = direction * i + this.m_PlayerPos;
                checkDirection = m_chessBoard.isSquareAvailable(squareToCheck, this, pieceMoveRestriction, bSameColorOverride);

                if(!checkDirection) {break;}
                

                availMoves.Add(squareToCheck);
                i++;

                ChessPiece chessPieceToCheck = m_chessBoard.GetChessPieceInThisSquare(squareToCheck);

                if (chessPieceToCheck != null && m_chessBoard.isSquareOccupied(squareToCheck, this))
                {
                    if (bSameColorOverride)
                    {
                        if (bIgnoreFirstKingCheck && chessPieceToCheck.getPieceType() == PieceType.King)
                        {
                            bIgnoreFirstKingCheck = false;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        if (bFirstCheck)
        {
            RemoveAvailableMovesIfTheyResultInACheck(ref availMoves);
        }

        return availMoves;
    }

    //TODO: 
    public virtual void movePiece(Vector2Int newPos, bool bFakeMove)
    {
        //Check to see if another piece is already on the square in question. 
        //Move piece to new location.
        float posX = newPos.x * m_chessBoard.m_SquareSize.x + m_chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = newPos.y * m_chessBoard.m_SquareSize.y + m_chessBoard.m_BoardStartPos.transform.position.z;
        
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

    // This function physically does every possible move of the piece you have selected, it checks if your king is check,
    // if it is in check after the move, then remove that available move. Once the verification is done, move the piece back to it's original position
    // like nothing happened. This happens very fast so you will not see it, but it could be slowed down. If you slow it down it could be 
    // useful in the future to visually see and debug the bot trying every possible move, but this function might need to be replaced with something 
    // more efficient in the future.
    public void RemoveAvailableMovesIfTheyResultInACheck(ref List<Vector2Int> availableMoves)
    {
        m_chessBoard.DisplayMoveSquares(this, availableMoves);

        for (int i = 0; i < m_chessBoard.m_AvailbleSquares.Count; i++)
        {
            Vector2Int oldPosition = getCurrentPos();
            ChessPiece lastChessPieceSelected = m_chessBoard.m_CurrentPieceSelection;
            ChessPiece killedPiece = m_chessBoard.MovePiece(m_chessBoard.m_AvailbleSquares[i], true);
            Vector2Int destination = getCurrentPos();

            m_chessBoard.m_CurrentPieceSelection = m_chessBoard.GetChessPieceInThisSquare(new Vector2Int(destination.x, destination.y));

            bool bDoRemove = false;
            if (m_chessBoard.CheckIfCheck(m_PlayerOrientation))
            {
                bDoRemove = true;
            }

            m_chessBoard.squares[destination.x, destination.y].removePiece(false, true);
            m_chessBoard.squares[oldPosition.x, oldPosition.y].setPiece(this);
            this.movePiece(oldPosition, true);

            if (killedPiece != null)
            {
                killedPiece.movePiece(new Vector2Int(destination.x, destination.y), true);
                m_chessBoard.squares[destination.x, destination.y].setPiece(killedPiece);
                killedPiece.enabled = true;
                killedPiece.m_Moveable = true;
            }

            if (bDoRemove)
            {
                int unusedVar = availableMoves.Remove(availableMoves.Find(move => move == destination)) ? i = -1 : 0;
            }
            m_chessBoard.DisplayMoveSquares(this, availableMoves);
        }
    }

    public virtual bool TryAddingAvailableMove(ref List<Vector2Int> availableMoves, int x, int y, PieceMoveRestriction pieceMoveRestriction, bool bSameColorOverride = false)
    {
        Vector2Int newMove = new Vector2Int(m_PlayerPos.x + x * m_PlayerOrientation, m_PlayerPos.y + y * m_PlayerOrientation);
        if (m_chessBoard.isSquareAvailable(newMove, this, pieceMoveRestriction, bSameColorOverride))
        {
            availableMoves.Add(newMove);
            return true;
        }
        return false;
    }
}
