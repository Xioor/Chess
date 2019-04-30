using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    ChessBoard m_ChessBoard;

    public GameObject m_WhitePawnPrefab;
    public GameObject m_BlackPawnPrefab;
    public GameObject m_BoardStartPos;
    public GameObject m_BoardEndPos;
    // Start is called before the first frame update
    void Start()
    {
        m_ChessBoard = ChessBoard.getInstance();
        m_ChessBoard.m_WhitePawnPrefab = m_WhitePawnPrefab;
        m_ChessBoard.m_BlackPawnPrefab = m_BlackPawnPrefab;
        m_ChessBoard.m_BoardStartPos = m_BoardStartPos;
        m_ChessBoard.m_BoardEndPos = m_BoardEndPos;
        m_ChessBoard.generateNewBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
