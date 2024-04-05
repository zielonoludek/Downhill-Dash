using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    void Start()
    {
        StartCoroutine(SwitchCanvasAfterDelay(2f));
        Debug.Log("Start");
    }

    IEnumerator SwitchCanvasAfterDelay(float delay)
    {
        canvas1.SetActive(true);
        
        yield return new WaitForSeconds(delay);

        canvas1.SetActive(false);

        canvas2.SetActive(true);
        
        yield return new WaitForSeconds(delay);
        
        canvas2.SetActive(false);
        GameManager.instance.levelManager.LoadMainMenu();
    }
}