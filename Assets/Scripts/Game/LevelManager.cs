using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public void LoadNextLevel(PlayMode playMode)
    {
        int currentLevelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex+1 < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            GameManager.instance.SetCurrentPlayMode(playMode);
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevelIndex+1);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    
    public void ReloadLevel(PlayMode playMode)
    {
        GameManager.instance.SetCurrentPlayMode(playMode);
        int currentLevelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevelIndex);
    }
    
    
    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void LoadIntroScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }

    public void LoadLevel(int levelIndex, PlayMode playMode)
    {
        GameManager.instance.SetCurrentPlayMode(playMode);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }
}
