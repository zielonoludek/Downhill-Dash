using UnityEngine;

public class BoardSelection : MonoBehaviour
{
    public GameObject[] boards;
    public int selectedBoard;

    public void NextBoard()
    {
        boards[selectedBoard].SetActive(false);
        selectedBoard = (selectedBoard + 1) % boards.Length;
        boards[selectedBoard].SetActive(true);
    }
    public void PreviousBoard()
    {
        boards[selectedBoard].SetActive(false);
        selectedBoard--;
        if (selectedBoard < 0)
        {
            selectedBoard += boards.Length;
        }
        boards[selectedBoard].SetActive(true);
    }
    public void SaveBoardState()
    {
        PlayerPrefs.SetInt("selectedBoard", selectedBoard);
    }
   
}
