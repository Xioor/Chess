/*

This class is a singleton that holds all of the info for the board. 
This will hold:
    Piece location. 

 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessBoard : MonoBehaviour
{  
    public GameObject m_WhitePawnPrefab;
    public GameObject m_BlackPawnPrefab;
    public GameObject m_BoardStartPos;
    public GameObject m_BoardEndPos;
    public GameObject m_AvailableSquarePrefab;

    public Vector2 m_SquareSize;

    ChessPiece m_CurrentPieceSelection;
    List<GameObject> m_AvailbleSquares;

    static ChessBoard m_Instace = null;

    public static ChessBoard getInstance()
    {
        return m_Instace;
    }
    
    void Awake()
    {
        if(m_Instace == null)
        {
            m_Instace = this;
        }
    }

    private ChessBoard ()
    {
       // generateNewBoard();
    }
    
    // Start is called before the first frame update
    Vector3 squareLocations;
    Square[,] squares = new Square[8,8];

    public bool isSquareAvailable(Vector2Int square, ChessPiece pieceToMove, bool bAvaiableOnlyIfKillPossible = false)
    {
        //Check if square is in bounds.
        if(square.x > 7 || square.x < 0 || square.y > 7 || square.y < 0)
            return false; //Out of bounds

        Square squareDestination = squares[square.x, square.y];

        if (squareDestination.isOccupied && squareDestination.piece.getOrientation() != pieceToMove.getOrientation())
        {
            return true;
        }
        else if(!squareDestination.isOccupied && !bAvaiableOnlyIfKillPossible)
        {
            return true; 
        }

        return false;
    }

    public void DisplayMoveSquares(ChessPiece currentPiece, List<Vector2Int> SquaresAvail)
    {
        if (m_CurrentPieceSelection == currentPiece)
            return;

        if(m_AvailbleSquares != null)
        {
            foreach(GameObject square in m_AvailbleSquares)
            {
                Destroy(square);
            }
        }   
        
        m_AvailbleSquares = new List<GameObject>();
        m_CurrentPieceSelection = currentPiece;

        foreach(Vector2Int square in SquaresAvail)
        {
            GameObject availSquare = Instantiate(m_AvailableSquarePrefab); 
            availSquare.GetComponent<AvailableSquare>().SetInfo(square);

            float posX = square.x * m_SquareSize.x + m_BoardStartPos.transform.position.x;
            float posZ = square.y * m_SquareSize.y + m_BoardStartPos.transform.position.z;

            Vector3 squarePos = new Vector3(posX, availSquare.transform.position.y ,posZ);
            availSquare.transform.position = squarePos;

            m_AvailbleSquares.Add(availSquare);
        }
    }

    public void MovePiece(GameObject selectedSquare)
    {
        Vector2Int currentPos = m_CurrentPieceSelection.getCurrentPos();
        Vector2Int moveToPos = selectedSquare.GetComponent<AvailableSquare>().GetInfo();
        
        squares[currentPos.x, currentPos.y].removePiece();
        squares[moveToPos.x, moveToPos.y].setPiece(m_CurrentPieceSelection);

        m_CurrentPieceSelection.movePiece(moveToPos);
    }

    public void saveCurrentBoard()
    {
        List<BoardLayoutInfo> boardLayout = new List<BoardLayoutInfo>();

        //Go through all squares, and extract data
        foreach(Square square in squares)
        {
            //check if square contains a Chess Piece.
            if(square.isOccupied)
            {
                //Add item to board layout
                BoardLayoutInfo thisSquare = new BoardLayoutInfo();
                thisSquare.Orientation = square.piece.getOrientation();
                thisSquare.StartPos = square.piece.getStartPos();
                thisSquare.type = square.piece.getPieceType();

                boardLayout.Add(thisSquare);
            }
        }

        //Save list as json. 
        
    }

    public void generateNewBoard()
    {
    //TODO: add logic to generate new board.
       
    //Load Json file. 
        //Data needed for instantiation. 
            // Vector2Int StartPos
            // Type of piece
            // Orientation
    //Parse Json file
        //
    
    //Get Board size
    m_SquareSize.x = Mathf.Abs((m_BoardEndPos.transform.position.x - m_BoardStartPos.transform.position.x) / 7);
    m_SquareSize.y = Mathf.Abs((m_BoardEndPos.transform.position.z - m_BoardStartPos.transform.position.z) / 7);

    //Instantiate all of the GameObjects.
        //Instantiate pawns
        for(int i = 0; i < 8; i++)
        {
            //Instantiate Pawn object.
            GameObject whitePawnObject = Instantiate(m_WhitePawnPrefab); 
            GameObject blackPawnObject = Instantiate(m_BlackPawnPrefab);

            //Set Pawn default values.
            Pawn whitePawnScript = whitePawnObject.GetComponent<Pawn>();
            Pawn blackPawnScript = blackPawnObject.GetComponent<Pawn>();

            Vector2Int whitePawnPos = new Vector2Int(1, i);
            Vector2Int blackPawnPos = new Vector2Int(6, i);

            whitePawnScript.SetStartInfo(whitePawnPos, 1);
            blackPawnScript.SetStartInfo(blackPawnPos, -1);

            squares[whitePawnPos.x, whitePawnPos.y].setPiece(whitePawnScript);
            squares[blackPawnPos.x, blackPawnPos.y].setPiece(blackPawnScript);
        }
    }
}

public enum PieceType
{
    Pawn, 
    Rook,
    Bishop,
    Knight,
    Queen,
    King
}

[System.Serializable]
struct BoardLayoutInfo
{
    public Vector2Int StartPos;

    public PieceType type;
    public int Orientation;
}

struct Square
{
    Vector2Int gridLocation;
    public ChessPiece piece;    
    public bool isOccupied;

    public void setPiece(ChessPiece piece)
    {
        isOccupied = true;
        this.piece = piece;
    }

    public void removePiece()
    {
        isOccupied = false;
        piece = null;
    }
}


