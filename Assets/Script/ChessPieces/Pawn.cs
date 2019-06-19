using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    bool m_bFirstMove = true;
    
    public override PieceType getPieceType()
    {
        return PieceType.Pawn;
    }

    public override void SetStartInfo(Vector2Int intialPos, int orientaion)
    {
        base.SetStartInfo(intialPos, orientaion);
        m_bFirstMove = true;
    }

    public override void movePiece(Vector2Int newPos, bool bFakeMove)
    {
        //Check to see if another piece is already on the square in question. 
        //Move piece to new location.
        float posX = newPos.x * m_chessBoard.m_SquareSize.x + m_chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = newPos.y * m_chessBoard.m_SquareSize.y + m_chessBoard.m_BoardStartPos.transform.position.z;
        
        transform.position = new Vector3(posX, transform.position.y, posZ);
        m_PlayerPos = newPos;

        if (!bFakeMove)
        {
            m_bFirstMove = false;
        }
    }

    public override List<Vector2Int> getAvailableMoves(List<Vector2Int> squaresOverride = default(List<Vector2Int>), PieceMoveRestriction pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, bool bSameColorOverride = false, bool bFirstCheck = false)
    {
        //Pawns are only able to move forward two squares on their first move, and one square forward on their subsquent moves. They can also only kill other pieces
        //one square forward diagonally to the left or right and they can only do that move while killing opponents piece.
        List<Vector2Int> availableMoves = new List<Vector2Int>();

        if (squaresOverride != null && squaresOverride.Count > 0)
        {
            TryAddingAvailableMove(ref availableMoves, 1, 1, PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, bSameColorOverride);
            TryAddingAvailableMove(ref availableMoves, 1, -1, PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, bSameColorOverride);
        }
        else
        {
            if (TryAddingAvailableMove(ref availableMoves, 1, 0, PieceMoveRestriction.OnlyWhenPositionFree) && m_bFirstMove)
            {
                TryAddingAvailableMove(ref availableMoves, 2, 0, PieceMoveRestriction.OnlyWhenPositionFree);
            }
            TryAddingAvailableMove(ref availableMoves, 1, 1, PieceMoveRestriction.OnlyWhenPositionOccupiedByOpponent, bSameColorOverride);
            TryAddingAvailableMove(ref availableMoves, 1, -1, PieceMoveRestriction.OnlyWhenPositionOccupiedByOpponent, bSameColorOverride);
        }

        if (bFirstCheck)
        {
            RemoveAvailableMovesIfTheyResultInACheck(ref availableMoves);
        }

        return availableMoves;
    }
}