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
#region prefabs
    public GameObject m_WhitePawnPrefab;
    public GameObject m_BlackPawnPrefab;
    public GameObject m_WhiteRookPrefab;
    public GameObject m_BlackRookPrefab;
    public GameObject m_WhiteBishopPrefab;
    public GameObject m_BlackBishopPrefab;
    public GameObject m_WhiteKnightPrefab;
    public GameObject m_BlackKnightPrefab;
    public GameObject m_WhiteQueenPrefab;
    public GameObject m_BlackQueenPrefab;
    public GameObject m_WhiteKingPrefab;
    public GameObject m_BlackKingPrefab;
#endregion
 
    public GameObject m_BoardStartPos;
    public GameObject m_BoardEndPos;
    public GameObject m_DeadPieceWhitePos;
    public GameObject m_DeadPieceBlackPos;
    public GameObject m_AvailableSquarePrefab;

    public Vector2 m_SquareSize;

    public int m_DeadWhitePieces;
    public int m_DeadBlackPieces;

    public ChessPiece m_CurrentPieceSelection;
    public List<GameObject> m_AvailbleSquares;

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
        m_DeadWhitePieces = 0;
        m_DeadBlackPieces = 0;
    }
    
    // Start is called before the first frame update
    Vector3 squareLocations;
    Square[,] squares = new Square[8,8];

    public bool isSquareOccupied(Vector2Int square, ChessPiece pieceToMove)
    {
        Square squareDestination = squares[square.x, square.y];
         if (squareDestination.isOccupied)
        {
            return true;
        }
        return false;
    }

    public bool isSquareAvailable(Vector2Int square, ChessPiece pieceToMove, PieceMoveRestriction pieceMoveRestriction = PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent)
    {
        //Check if square is in bounds.
        if(square.x > 7 || square.x < 0 || square.y > 7 || square.y < 0)
            return false; //Out of bounds

        Square squareDestination = squares[square.x, square.y];

        if (squareDestination.isOccupied)
        {
            if (squareDestination.piece.getOrientation() != pieceToMove.getOrientation() && 
                pieceMoveRestriction <= PieceMoveRestriction.OnlyWhenPositionOccupiedByOpponent)
            {
                return true;
            }
        }
        else
        {
            if (pieceMoveRestriction == PieceMoveRestriction.OnlyWhenPositionFreeOrOccupiedByOpponent ||
                pieceMoveRestriction == PieceMoveRestriction.OnlyWhenPositionFree)
            {
                return true; 
            }
        }

        return false;
    }

    public void DisplayMoveSquares(ChessPiece currentPiece, List<Vector2Int> SquaresAvail)
    {
        if (m_CurrentPieceSelection == currentPiece)
        {
            ResetAvailableSquares();
            return;
        }


        if (m_AvailbleSquares != null)
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

    public void MovePiece(GameObject selectedSquareOrPiece)
    {
        Vector2Int currentPos = m_CurrentPieceSelection.getCurrentPos();
        Vector2Int moveToPos;

        if (selectedSquareOrPiece.GetComponent<ChessPiece>() != null)
        {
            moveToPos = selectedSquareOrPiece.GetComponent<ChessPiece>().getCurrentPos();
        }
        else
        {   
            moveToPos = selectedSquareOrPiece.GetComponent<AvailableSquare>().GetInfo();
        }

        squares[currentPos.x, currentPos.y].removePiece(false);
        squares[moveToPos.x, moveToPos.y].removePiece(true);

        squares[moveToPos.x, moveToPos.y].setPiece(m_CurrentPieceSelection);

        m_CurrentPieceSelection.movePiece(moveToPos);
        ResetAvailableSquares();
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

        //Instatiate Rooks
        InstatiateChessPiece(m_WhiteRookPrefab, 0, 0, 1);
        InstatiateChessPiece(m_WhiteRookPrefab, 0, 7, 1);
        InstatiateChessPiece(m_WhiteRookPrefab, 7, 0, -1);
        InstatiateChessPiece(m_WhiteRookPrefab, 7, 7, -1);
        
        //Instatiate Knights
        InstatiateChessPiece(m_WhiteKnightPrefab, 0, 1, 1);
        InstatiateChessPiece(m_WhiteKnightPrefab, 0, 6, 1);
        InstatiateChessPiece(m_BlackKnightPrefab, 7, 1, -1);
        InstatiateChessPiece(m_BlackKnightPrefab, 7, 6, -1);
        
        //Instatiate Bishops
        InstatiateChessPiece(m_WhiteBishopPrefab, 0, 2, 1);
        InstatiateChessPiece(m_WhiteBishopPrefab, 0, 5, 1);
        InstatiateChessPiece(m_BlackBishopPrefab, 7, 2, -1);
        InstatiateChessPiece(m_BlackBishopPrefab, 7, 5, -1);

        //Instatiate Queens
        InstatiateChessPiece(m_WhiteQueenPrefab, 0, 3, 1);
        InstatiateChessPiece(m_BlackQueenPrefab, 7, 3, -1);

        //Instatiate Kings
        InstatiateChessPiece(m_WhiteKingPrefab, 0, 4, 1);
        InstatiateChessPiece(m_BlackKingPrefab, 7, 4, -1);
    }


    void InstatiateChessPiece(GameObject m_PiecePrefab, int posX, int posY, int dir)
    {
        //Instantiate Pawn object.
            GameObject PieceObject = Instantiate(m_PiecePrefab); 

            //Set Pawn default values.
            ChessPiece PieceScript = PieceObject.GetComponent<ChessPiece>();
            Vector2Int pos = new Vector2Int(posX, posY);

            PieceScript.SetStartInfo(pos, dir);

            squares[pos.x, pos.y].setPiece(PieceScript);
    }

    void InstatiateChessPiece(GameObject m_PiecePrefab, Vector2Int pos, int dir)
    {
        //Instantiate Pawn object.
            GameObject PieceObject = Instantiate(m_PiecePrefab); 

            //Set Pawn default values.
            ChessPiece PieceScript = PieceObject.GetComponent<ChessPiece>();

            PieceScript.SetStartInfo(pos, dir);

            squares[pos.x, pos.y].setPiece(PieceScript);
    }

    public void ResetAvailableSquares()
    {
        m_CurrentPieceSelection = null;
        foreach (GameObject square in m_AvailbleSquares)
        {
            Destroy(square);
        }
        m_AvailbleSquares.Clear();
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

public enum PieceMoveRestriction
{
    OnlyWhenPositionFreeOrOccupiedByOpponent,
    OnlyWhenPositionOccupiedByOpponent,
    OnlyWhenPositionFree
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

    public void removePiece(bool bKill)
    {
        ChessBoard chessBoard = ChessBoard.getInstance();
        Vector3 deadWhitePiecesPosition = chessBoard.m_DeadPieceWhitePos.transform.position;
        Vector3 deadBlackPiecesPosition = chessBoard.m_DeadPieceWhitePos.transform.position;

        if (isOccupied == true && bKill)
        {
            if (piece.getOrientation() == 1)
            {
                chessBoard.m_DeadWhitePieces++;
                piece.transform.position = new Vector3(deadWhitePiecesPosition.x,
                                                       deadWhitePiecesPosition.y,
                                                       deadWhitePiecesPosition.z + (chessBoard.m_DeadWhitePieces * chessBoard.m_SquareSize.y / 2));
            }
            else
            {
                chessBoard.m_DeadBlackPieces++;
                piece.transform.position = new Vector3(deadBlackPiecesPosition.x,
                                                       deadBlackPiecesPosition.y,
                                                       deadBlackPiecesPosition.z + (chessBoard.m_DeadBlackPieces * chessBoard.m_SquareSize.y / 2));
            }
            piece.m_Moveable = false;
        }
        isOccupied = false;
        if (piece != null)
        {
            piece.enabled = false;
            piece = null;
        }
    }
}


