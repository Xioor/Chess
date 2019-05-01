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
                    m_CurrentChessPiece = hit.collider.gameObject.GetComponent<ChessPiece>();

                    List<Vector2Int> availMoves = m_CurrentChessPiece.getAvailableMoves();

                    //Create current move squares

                    if(availMoves != null)
                    {
                        ChessBoard.getInstance().DisplayMoveSquares(m_CurrentChessPiece, availMoves);
                    }

                    Debug.Log(availMoves);
                }

                //Check if it is a move 
                if(hit.collider.tag == "MoveSquare")
                {
                   // Get the squares grid location. 
                   // m_CurrentChessPiece.movePiece();
                   ChessBoard.getInstance().MovePiece(hit.collider.gameObject);
                }
                

               // Debug.Log("Object touched");
            }
        }
    }
}
