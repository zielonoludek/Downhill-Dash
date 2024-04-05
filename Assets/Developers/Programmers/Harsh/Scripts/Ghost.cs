using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    GameModeHandler gameModeHandler;
    SimulatedPlayer simulatedPlayer;
    
    bool isRecording = false;
    bool isPlayingBack = false;
    
    List<Vector3> recordedPositions = new List<Vector3>();
    List<Quaternion> recordedRotations = new List<Quaternion>();
    
    float framecount = 0;
    float framelimit = 1;

    private void Awake()
    {
        gameModeHandler = GameObject.FindFirstObjectByType<GameModeHandler>();
        simulatedPlayer = GameObject.FindFirstObjectByType<SimulatedPlayer>();
    }

    void OnEnable()
    {
        gameModeHandler.OnRoundStart += StartRecording;
        gameModeHandler.OnRoundEnd += StopRecording;
        gameModeHandler.OnRoundTwoStart += StartPlayback;
        gameModeHandler.OnRoundTwoEnd += StopPlayback;
    }

    private void StopPlayback()
    {
        isPlayingBack = false;
    }

    private void StartPlayback()
    {
        Debug.Log("Start playback");
        isPlayingBack = true;
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void StopRecording()
    {
        isRecording = false;
    }

    private void StartRecording()
    {
        GetComponent<MeshRenderer>().enabled = false;
        isRecording = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        framecount++;
        if (framecount > framelimit && Time.timeScale > 0)
        {
            framecount = 0;
            if (isRecording)
            {
                recordedPositions.Add(simulatedPlayer.transform.position);
                recordedRotations.Add(simulatedPlayer.transform.rotation);
            }
            else if (isPlayingBack)
            {
                if (recordedPositions.Count > 0)
                {
                    //transform.position = Vector3.Lerp(transform.position, recordedPositions[0], Time.deltaTime * 10);
                    //transform.rotation = Quaternion.Lerp(transform.rotation, recordedRotations[0], Time.deltaTime * 10);
                    recordedPositions.RemoveAt(0);
                    recordedRotations.RemoveAt(0);
                }
            }
        }
        else if (isPlayingBack)
        {
            if (recordedPositions.Count > 0)
            {
                transform.position = Vector3.Lerp(transform.position, recordedPositions[0], framecount / framelimit);
                transform.rotation = Quaternion.Lerp(transform.rotation, recordedRotations[0], framecount / framelimit);
            }
        }
        
    }
}
