using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

enum PlayerTurn
{
    None,
    Player1,
    Player2
}

enum HandlerState
{
    onboarding,
    roundOne,
    roundTwo,
    FinishScreen
}

public class GameModeHandler : MonoBehaviour
{
    
    // events
    public delegate void RoundStart();
    public  event RoundStart OnRoundStart;
    
    public delegate void RoundEnd();
    public  event RoundEnd OnRoundEnd;
    
    public delegate void RoundTwoStart();
    public  event RoundTwoStart OnRoundTwoStart;
    
    public delegate void RoundTwoEnd();
    public  event RoundTwoEnd OnRoundTwoEnd;
    
    
    
    // 
    private PlayMode currentPlayMode;
    private PlayerTurn currentPlayerTurn;
    private HandlerState currentHandlerState;
    
    //input manager
    private InputManager inputManager;
    
    // UI
    
    [SerializeField] private GameObject OnboardingCanvas;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject PlayerNumberUI;
    [SerializeField] private GameObject TimerUI;
    [SerializeField] private GameObject SoloFinishUI;
    [SerializeField] private GameObject Player1FinishUI;
    [SerializeField] private GameObject Player2FinishUI;
    
    public TextMeshProUGUI SoloFinishTime;
    public TextMeshProUGUI Player1FinishTime;
    public TextMeshProUGUI Player1FinishTimeRoundTwo;
    public TextMeshProUGUI Player2FinishTime;

    public TextMeshProUGUI PlayerWinText;
    
    float timer = 0.0f;
    float player1Time = 0.0f;
    float player2Time = 0.0f;


    private void Awake()
    {
        inputManager = FindFirstObjectByType<InputManager>();
    }

    private void OnEnable()
    {
        
        inputManager.OnSwipeUp += StartRound;
        
    }

    public void StartRound()
    {
        if (currentHandlerState == HandlerState.onboarding)
        {
            timer = 0.0f;
            Time.timeScale = 1f;
            OnboardingCanvas.SetActive(false);
            PlayerNumberUI.SetActive(true);
            TimerUI.SetActive(true);
            HUD.SetActive(true);
            SoloFinishUI.SetActive(false);
            Player1FinishUI.SetActive(false);
            Player2FinishUI.SetActive(false);
            

            switch (currentPlayerTurn)
            {
                case PlayerTurn.None:
                    currentPlayerTurn = PlayerTurn.Player1;
                    currentHandlerState = HandlerState.roundOne;
                    PlayerNumberUI.GetComponent<TextMeshProUGUI>().text = "Player 1";
                    OnRoundStart?.Invoke();
                    break;
                case PlayerTurn.Player1:
                    currentPlayerTurn = PlayerTurn.Player2;
                    currentHandlerState = HandlerState.roundTwo;
                    PlayerNumberUI.GetComponent<TextMeshProUGUI>().text = "Player 2";
                    OnRoundTwoStart?.Invoke();
                    break;
                case PlayerTurn.Player2:
                    //currentPlayerTurn = PlayerTurn.Player1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        currentPlayMode = GameManager.instance.GetCurrentPlayMode();
        currentPlayerTurn = PlayerTurn.None;
        
        currentHandlerState = HandlerState.onboarding;
        Onboarding();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentHandlerState)
        {
            case HandlerState.onboarding:
                break;
            case HandlerState.roundOne:
                UpdateTimer();
                break;
            case HandlerState.roundTwo:
                UpdateTimer();
                break;
            case HandlerState.FinishScreen:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    
    void Onboarding()
    {
        Time.timeScale = 0f;
        OnboardingCanvas.SetActive(true);
        PlayerNumberUI.SetActive(false);
        TimerUI.SetActive(false);
        Player1FinishUI.SetActive(false);
        Player2FinishUI.SetActive(false);
        HUD.SetActive(false);
    }
    
    
    void UpdateTimer()
    {
        timer += Time.deltaTime;
        TimerUI.GetComponent<TextMeshProUGUI>().text = timer.ToString("F2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            switch (currentHandlerState)
            {
                case HandlerState.onboarding:
                    break;
                case HandlerState.roundOne:
                    player1Time = timer;
                    OnRoundEnd?.Invoke();
                    FinishScreen();
                    break;
                case HandlerState.roundTwo:
                    player2Time = timer;
                    OnRoundTwoEnd?.Invoke();
                    FinishScreen();
                    break;
                case HandlerState.FinishScreen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private void FinishScreen()
    {
        currentHandlerState = HandlerState.FinishScreen;
        Time.timeScale = 0f;
        switch (currentPlayerTurn)
        {
            case PlayerTurn.None:
                break;
            case PlayerTurn.Player1:
                if (currentPlayMode == PlayMode.SinglePlayer)
                {
                    SoloFinishUI.SetActive(true);
                    SoloFinishTime.text = player1Time.ToString("F2");
                }
                else if (currentPlayMode == PlayMode.TwoPlayer)
                {
                    Player1FinishUI.SetActive(true);
                    Player1FinishTime.text = player1Time.ToString("F2");
                }
                break;
            case PlayerTurn.Player2:
                Player2FinishUI.SetActive(true);
                Player1FinishTimeRoundTwo.text = player1Time.ToString("F2");
                Player2FinishTime.text = player2Time.ToString("F2");
                if (player2Time < player1Time)
                {
                    Player2FinishTime.color = Color.green;
                    PlayerWinText.text = "Player 2 Wins!";
                }
                else
                {
                    Player1FinishTime.color = Color.green;
                    PlayerWinText.text = "Player 1 Wins!";
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void NextOnFinish()
    {
        Time.timeScale = 1f;
        switch(currentPlayMode)
        {
            case PlayMode.SinglePlayer:
                SceneManager.LoadScene(0);
                break;
            case PlayMode.TwoPlayer:
                switch (currentPlayerTurn)
                {
                    case PlayerTurn.None:
                        break;
                    case PlayerTurn.Player1:
                        Player1FinishUI.SetActive(false);
                        currentHandlerState = HandlerState.onboarding;
                        Onboarding();
                        break;
                    case PlayerTurn.Player2:
                        SceneManager.LoadScene(0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
