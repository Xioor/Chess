using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
       public override PieceType getPieceType()
    {
        return PieceType.Knight;
    }

    public override List<Vector2Int> getAvailableMoves(List<Vector2Int> squaresOverride = default(List<Vector2Int>), PieceMoveRestriction pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, bool bCheckSameColor = false, bool bFirstCheck = false)
    {
        List<Vector2Int> availableMoves = new List<Vector2Int>();

        TryAddingAvailableMove(ref availableMoves, 2, 1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 2, -1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 1, 2, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, 1, -2, pieceMoveRestriction, bCheckSameColor);

        TryAddingAvailableMove(ref availableMoves, -2, -1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, -2, 1, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, -1, -2, pieceMoveRestriction, bCheckSameColor);
        TryAddingAvailableMove(ref availableMoves, -1, 2, pieceMoveRestriction, bCheckSameColor);

        if (bFirstCheck)
        {
            RemoveAvailableMovesIfTheyResultInACheck(ref availableMoves);
        }

        return availableMoves;
    }
}
