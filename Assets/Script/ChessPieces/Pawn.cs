using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    bool m_bFirstMove = true;

    public override List<Vector2Int> getAvailableMoves()
    {
        //Pawns are only able to move forward two squares on their first move, and one square forward on their subsquent moves. They can also only kill other pieces
        //one square forward diagonally to the left or right and they can only do that move while killing opponents piece.
        List<Vector2Int> availableMoves = new List<Vector2Int>();
        if (m_bFirstMove)
        {
            TryAddingAvailableMove(ref availableMoves, 0, 2, false);
        }
        TryAddingAvailableMove(ref availableMoves, 0, 1, false);
        TryAddingAvailableMove(ref availableMoves, 1, 1, true);
        TryAddingAvailableMove(ref availableMoves, -1, 1, true);
        return availableMoves;
    }
}