using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IngameInput : MonoBehaviour
{
    // Start is called before the first frame update

    //GameData gameData;
    int m_CurrentPlayerOrientation;
    ChessBoard m_chessBoard;

    void Start()
    {
        m_chessBoard = ChessBoard.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown( KeyCode.Escape))
        {
            GameManger.getInstance().togglePauseState();
        }
        
        if(GameManger.getInstance().GetPauseState() || GameManger.getInstance().GetGameOver())
        {
            return;
        }

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

                    //Check if the player is correct orientation according to GameManager
                    int playerTurn = GameManger.getInstance().GetPlayerTurn();

                    if(playerTurn != currentChessPiece.getOrientation() && m_chessBoard.m_CurrentPieceSelection == null)
                    {
                        //Do nothing
                        return;
                    }

                    if (currentChessPiece.m_Moveable)
                    {
                        if (m_chessBoard.m_CurrentPieceSelection == null || m_chessBoard.m_CurrentPieceSelection.getOrientation() == currentChessPiece.getOrientation())
                        {
                            m_chessBoard.m_CurrentPieceSelection = currentChessPiece;
                        }
                        else if (m_chessBoard.m_CurrentPieceSelection.getOrientation() != currentChessPiece.getOrientation() &&
                                 m_chessBoard.m_CurrentPieceSelection != hit.collider.gameObject.GetComponent<ChessPiece>())
                        {
                            List<GameObject> availableSquares = m_chessBoard.m_AvailbleSquares;
                            for (int i = 0; i < availableSquares.Count; i++)
                            {
                                if (availableSquares[i].GetComponent<AvailableSquare>() != null && hit.collider.gameObject.GetComponent<ChessPiece>() != null)
                                {
                                    if (availableSquares[i].GetComponent<AvailableSquare>().GetInfo() == hit.collider.gameObject.GetComponent<ChessPiece>().getCurrentPos())
                                    {
                                        m_chessBoard.MovePiece(hit.collider.gameObject, false);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_chessBoard.ResetAvailableSquares();
                            return;
                        }

                        List<Vector2Int> availMoves = m_chessBoard.m_CurrentPieceSelection.getAvailableMoves(null, PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent, false, true);

                        //Create current move squares
                        if (availMoves != null)
                        {
                            m_chessBoard.DisplayMoveSquares(m_chessBoard.m_CurrentPieceSelection, availMoves);
                        }

                        Debug.Log(availMoves);
                    }
                }

                //Check if it is a move 
                if(hit.collider.tag == "MoveSquare")
                {
                    m_chessBoard.MovePiece(hit.collider.gameObject, false);
                }
            }
        }
    }
}
