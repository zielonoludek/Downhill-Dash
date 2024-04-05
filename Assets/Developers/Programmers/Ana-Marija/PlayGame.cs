using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void StartGameSinglePlayer()
    {
       GameManager.instance.levelManager.LoadNextLevel(PlayMode.SinglePlayer);
    }
    public void StartGameTwoPlayer()
    {
        GameManager.instance.levelManager.LoadNextLevel(PlayMode.TwoPlayer);
    }
    
    public void SingleColombia()
    {
        GameManager.instance.levelManager.LoadLevel(1, PlayMode.SinglePlayer);
    }
    
    public void SingleNorway()
    {
        GameManager.instance.levelManager.LoadLevel(2, PlayMode.SinglePlayer);
    }
    
    public void VersusColombia()
    {
        GameManager.instance.levelManager.LoadLevel(1, PlayMode.TwoPlayer);
    }
    
    public void VersusNorway()
    {
        Debug.Log("VersusNorway");
        GameManager.instance.levelManager.LoadLevel(2, PlayMode.TwoPlayer);
    }
}