using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnToCheckpoint : MonoBehaviour
{
    public Transform respawnLocation;
    public bool respawn = false;
   // public float speed = 1; 
    SimulatedPlayer playerPhysics;
    private Transform originalPlayerTransform;
    
    float timer = 0.0f;
    private float waitTime = 0.5f;
    bool isWaiting = false;
    
    GameModeHandler gameModeHandler;
    
    public Vector3 cameraOffset;

    private void Awake()
    {
        gameModeHandler = GameObject.FindFirstObjectByType<GameModeHandler>();
    }

    void Start()
    {
        originalPlayerTransform = transform;
        playerPhysics = GetComponent<SimulatedPlayer>();
    }
    
    void OnEnable()
    {
        //gameModeHandler.OnRoundEnd += ResetCheckpoint;
        //gameModeHandler.OnRoundTwoStart += ResetCheckpoint;
        //gameModeHandler.OnRoundTwoEnd += ResetCheckpoint;
    }

    private void ResetCheckpoint()
    {
        respawnLocation = originalPlayerTransform;
        transform.position = respawnLocation.position;
        transform.rotation = respawnLocation.rotation;
    }

    void Update()
    {
        if(respawn)
        {
            //RespawnPlayer();
        } 
        
        if(isWaiting)
        {
            
            timer += Time.deltaTime;
            //Camera.main.GetComponent<OvershootCamPrototype>().speedRatio = 1 - (timer / waitTime);
            
            if(timer > waitTime)
            {
                //playerPhysics.ChangePlayerState(PlayerState.Standing);
                isWaiting = false;
                timer = 0.0f;
                respawn = false;
                playerPhysics.StartGame();
            }
        }
    }

    public void RespawnPlayer()
    {
        gameObject.transform.position = respawnLocation.position;
        gameObject.transform.rotation = respawnLocation.rotation;
        //Camera.main.transform.position = transform.position + cameraOffset;
        playerPhysics.SetPlayerVelocity(Vector3.zero);
        
        isWaiting = true;
     
        //Vector3 startingVelocity = transform.forward * speed * Time.deltaTime;
        //playerPhysics.SetPlayerVelocity(startingVelocity);
         // Reset respawn flag after respawning
    }
           


}
