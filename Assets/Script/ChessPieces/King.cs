using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override PieceType getPieceType()
    {
        return PieceType.King;
    }

    public override void SetStartInfo(Vector2Int intialPos, int orientaion)
    {
        m_StartPos = intialPos;
        m_PlayerPos = intialPos;
        m_PlayerOrientation = orientaion;

        float posX = intialPos.x * chessBoard.m_SquareSize.x + chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = intialPos.y * chessBoard.m_SquareSize.y + chessBoard.m_BoardStartPos.transform.position.z;

        transform.position = new Vector3(posX, transform.position.y, posZ);
        m_Moveable = true;
    }

    public override List<Vector2Int> getAvailableMoves(List<Vector2Int> squaresOverride = default(List<Vector2Int>), PieceMoveRestriction pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, bool bCheckSameColor = false)
    {
        List<Vector2Int> availableMoves = new List<Vector2Int>();

        pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionCannotResultInPossibleDeath;
        if (squaresOverride != null && squaresOverride.Count > 0)
        {
            pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent;
        }

        TryAddingAvailableMove(ref availableMoves, 0, 1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 1, 0, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 1, 1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, -1, -1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, -1, 1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 1, -1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, -1, 0, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 0, -1, pieceMoveRestriction, bCheckSameColor);
        return availableMoves;
    }
}
