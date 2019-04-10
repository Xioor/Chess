using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    bool firstTurn = true;

    public override List<Vector2Int> getAvailableMoves()
    {
        List<Vector2Int> availMoves = new List<Vector2Int>();

        //implement logic to get moves. 
        //Pawns are only able to move forward, two squares on the first turn, and one on subsquent turns. 
        if(firstTurn)
        {
            //check two squares in front.
            Vector2Int newSquare = new Vector2Int(m_PlayerPos.x + 2 * m_PlayerOrientation, m_PlayerPos.y);
            if(chessBoard.isSquareAvailable(newSquare, this))
            {
                availMoves.Add(newSquare);
            }
        }

        foreach(Vector2Int squareToCheck in m_MovementOffsets)
        {
            if(chessBoard.isSquareAvailable(squareToCheck, this))
            {
                availMoves.Add(squareToCheck);
            }
        }        
        return availMoves;
    }
}
