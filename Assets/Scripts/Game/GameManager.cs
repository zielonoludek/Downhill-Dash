using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayMode
{
    SinglePlayer,
    TwoPlayer
}


public class GameManager : PersistentSingleton<GameManager>
{
    
    PlayMode currentPlayMode = PlayMode.SinglePlayer;
    protected override void Awake()
    {
        base.Awake();
        // Initialize Systems Managers
        instance.gameObject.AddComponent<LevelManager>();
        levelManager = instance.GetComponent<LevelManager>();
        
        // Initialize Game States
        currentState = playingState;
        currentState.EnterState();
    }
    
    
    // Systems Managers
    public LevelManager levelManager;
    

    // Game States
    
    public BaseGameState currentState;
    
    PlayingState playingState = new PlayingState();
    PauseState pauseState = new PauseState();
    PlayerFallState playerFallState = new PlayerFallState();
    
    void Start()
    {
        
    }
    
    void Update()
    {
        currentState.UpdateState();
       // Debug.Log(currentPlayMode);
    }
    
    public void ChangeState(BaseGameState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    
    public void SetCurrentPlayMode(PlayMode newPlayMode)
    {
        currentPlayMode = newPlayMode;
    }

    public PlayMode GetCurrentPlayMode()
    {
        return currentPlayMode;
    }
}