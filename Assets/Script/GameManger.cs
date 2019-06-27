using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    public bool UseGameTime;
    public float GameTimeInMinutes;
    public float TimeTillFirstMoveInitial;
    public Text WinnerText;
    public Text PlayerOneTimerUI;
    public Text PlayerTwoTimerUI;
    public Text TimeTillFirstMoveUI;
    public Button PlayAgainButton;
    public Button MainMenuButton;
    ChessBoard m_ChessBoard;

    int PlayerTurn = 1; 

    int Winner;

    float PlayerOneTimer;
    float PlayerTwoTimer;

    float TimeTillFirstMove;

    bool GamePaused = false;
    bool GameOver = false;

    static GameManger m_Instace = null;

    public static GameManger getInstance()
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


    // Start is called before the first frame update
    void Start()
    {
        m_ChessBoard = ChessBoard.getInstance();
        m_ChessBoard.generateNewBoard();

        if(UseGameTime)
        {
            PlayerOneTimer = MinutesToSeconds(GameTimeInMinutes);
            PlayerTwoTimer = PlayerOneTimer;
            EnableDrawTime();
        }        

        WinnerText.gameObject.SetActive(false);
        PlayAgainButton.gameObject.SetActive(false);
        MainMenuButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameOver)
        {
            if(GamePaused)
            {
                //We are paused
                DrawPauseScreen();
            }
            else if(UseGameTime)
            {
                 UpdatePlayerTimer();
                 DrawPlayerTimer();
            }
        }
        else
        {
            //Game is over and draw appropriate options 
            DrawGameOver();
        }
    }
    public void StartNextGame()
    {
        //Start new game. 
        WinnerText.gameObject.SetActive(false);
        PlayAgainButton.gameObject.SetActive(false);
        MainMenuButton.gameObject.SetActive(false);
        //Clear board. 
        m_ChessBoard.ClearCurrentBoard();   

        //Set up new board. 
        m_ChessBoard.generateNewBoard();

        //Reset Game state
        if(UseGameTime)
        {
            PlayerOneTimer = MinutesToSeconds(GameTimeInMinutes);
            PlayerTwoTimer = PlayerOneTimer;
            EnableDrawTime();
        }    

        GameOver = false;
        Winner = 0;
        PlayerTurn = 1;
    }

    public void togglePauseState()
    {
        GamePaused = !GamePaused;
    }
    public void EndGame(int winningPlayer)
    {
        GameOver = true;
        if(winningPlayer == 1)
        {
            //Player one wins
            Winner = 1;
        }
        else if (winningPlayer == -1)
        {
            //Player two wins.
            Winner = 2;
        }
        else
        {
            Winner = 0;
        }
        ChessBoard.getInstance().ResetAvailableSquares();
    }

    void DrawGameOver()
    {
        // Game over UI
        WinnerText.gameObject.SetActive(true);
        PlayAgainButton.gameObject.SetActive(true);
        MainMenuButton.gameObject.SetActive(true);

        var unusedVar = Winner == 0 ? WinnerText.text = "It's a Draw!" : WinnerText.text = "Player " + Winner + " is the Winner!";

        //In Game UI
        TimeTillFirstMoveUI.gameObject.SetActive(false);
        PlayerOneTimerUI.gameObject.SetActive(false);
        PlayerTwoTimerUI.gameObject.SetActive(false);
    }

    void DrawPauseScreen()
    {
        //Draw the pause UI
        
    }

    float MinutesToSeconds(float min)
    {
        return min * 60;
    }

    string TimeToString(float timeInSeconds)
    {
        float min = timeInSeconds / 60;
        float Seconds = timeInSeconds - 60 * (int)min;

        if(min > 1)
        {
            return ((int)min).ToString()+((int)Seconds).ToString(":00");
        }
        return ((int)min).ToString()+((int)Seconds).ToString(":00");
    }

    public int GetPlayerTurn()
    {
        return PlayerTurn;
    }

    public bool GetPauseState()
    {
        return GamePaused;
    }

    public bool GetGameOver()
    {
        return GameOver;
    }

    public void SwitchPlayer()
    {
        PlayerTurn *= -1;
        TimeTillFirstMove = 0;
    }

    void UpdatePlayerTimer()
    {
        if(TimeTillFirstMove > 0)
        {
            TimeTillFirstMove -= Time.deltaTime;
            return;
        }
        else 
        {
            TimeTillFirstMoveUI.gameObject.SetActive(false);
        }

        if(PlayerTurn == 1)
        {
            PlayerOneTimer -= Time.deltaTime;
            //Check if our timer still is valid. 
            if(PlayerOneTimer < 0)
            {
                //Player two wins
                EndGame(2);
            }    
        }
        else
        {
            PlayerTwoTimer -= Time.deltaTime;    
            //Check if our timer is valid.
             if(PlayerTwoTimer < 0)
            {
                //Player one wins
                EndGame(1);
            }    
        }
    }

    void DrawPlayerTimer()
    {
        TimeTillFirstMoveUI.text = ((int)TimeTillFirstMove).ToString();
        PlayerOneTimerUI.text = TimeToString(PlayerOneTimer);   
        PlayerTwoTimerUI.text = TimeToString(PlayerTwoTimer);
    }

    void EnableDrawTime()
    {
        //Enable the time gameobjects. 
        if(TimeTillFirstMove != null && TimeTillFirstMove > 0)
        {
            //enable time till first object
            TimeTillFirstMoveUI.gameObject.SetActive(true);
            TimeTillFirstMoveUI.text = ((int)TimeTillFirstMove).ToString();
        }

        PlayerOneTimerUI.gameObject.SetActive(true);
        PlayerOneTimerUI.text = ((int)PlayerOneTimer).ToString();         

        PlayerTwoTimerUI.gameObject.SetActive(true);
        PlayerTwoTimerUI.text = ((int)PlayerTwoTimer).ToString();
    }
}
