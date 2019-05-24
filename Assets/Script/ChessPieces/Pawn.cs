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

    public override void movePiece(Vector2Int newPos)
    {
        //Check to see if another piece is already on the square in question. 
        //Move piece to new location.
        float posX = newPos.x * chessBoard.m_SquareSize.x + chessBoard.m_BoardStartPos.transform.position.x;
        float posZ = newPos.y * chessBoard.m_SquareSize.y + chessBoard.m_BoardStartPos.transform.position.z;
        
        transform.position = new Vector3(posX, transform.position.y, posZ);
        m_PlayerPos = newPos;
        m_bFirstMove = false;
    }

    public override List<Vector2Int> getAvailableMoves()
    {
        //Pawns are only able to move forward two squares on their first move, and one square forward on their subsquent moves. They can also only kill other pieces
        //one square forward diagonally to the left or right and they can only do that move while killing opponents piece.
        List<Vector2Int> availableMoves = new List<Vector2Int>();
        if (TryAddingAvailableMove(ref availableMoves, 1, 0, PieceMoveRestriction.OnlyWhenPositionFree) && m_bFirstMove)
        {
            TryAddingAvailableMove(ref availableMoves, 2, 0, PieceMoveRestriction.OnlyWhenPositionFree);
        }
        TryAddingAvailableMove(ref availableMoves, 1, 1, PieceMoveRestriction.OnlyWhenPositionOccupiedByOpponent);
        TryAddingAvailableMove(ref availableMoves, 1, -1, PieceMoveRestriction.OnlyWhenPositionOccupiedByOpponent);
        return availableMoves;
    }
}