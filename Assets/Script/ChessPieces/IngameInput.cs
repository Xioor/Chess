using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IngameInput : MonoBehaviour
{
    // Start is called before the first frame update

    //GameData gameData;
    ChessPiece m_CurrentChessPiece;
    int m_CurrentPlayerOrientation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Check What we hit
                if(hit.collider.tag == "ChessPiece")
                {
                    ChessPiece currentChessPiece = hit.collider.gameObject.GetComponent<ChessPiece>();

                    if (currentChessPiece.m_Moveable)
                    {
                        if (m_CurrentChessPiece == null)
                        {
                            m_CurrentChessPiece = currentChessPiece;
                        }
                        else if (m_CurrentChessPiece.getOrientation() != currentChessPiece.getOrientation() &&
                                 m_CurrentChessPiece != hit.collider.gameObject.GetComponent<ChessPiece>())
                        {
                            List<GameObject> availableSquares = ChessBoard.getInstance().m_AvailbleSquares;
                            for (int i = 0; i < availableSquares.Count; i++)
                            {
                                if (availableSquares[i].GetComponent<AvailableSquare>() != null && hit.collider.gameObject.GetComponent<ChessPiece>() != null)
                                {
                                    if (availableSquares[i].GetComponent<AvailableSquare>().GetInfo() == hit.collider.gameObject.GetComponent<ChessPiece>().getCurrentPos())
                                    {
                                        ChessBoard.getInstance().MovePiece(hit.collider.gameObject);
                                        m_CurrentChessPiece = null;
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_CurrentChessPiece = null;
                            ChessBoard.getInstance().ResetAvailableSquares();
                            return;
                        }

                        List<Vector2Int> availMoves = m_CurrentChessPiece.getAvailableMoves();

                        //Create current move squares
                        if (availMoves != null)
                        {
                            ChessBoard.getInstance().DisplayMoveSquares(m_CurrentChessPiece, availMoves);
                        }

                        Debug.Log(availMoves);
                    }
                }

                //Check if it is a move 
                if(hit.collider.tag == "MoveSquare")
                {
                   ChessBoard.getInstance().MovePiece(hit.collider.gameObject);
                   m_CurrentChessPiece = null;
                }
            }
        }
    }
}
