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
}
