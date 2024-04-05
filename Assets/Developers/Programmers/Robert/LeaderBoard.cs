using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    int totalPlayers = 0;

    [SerializeField]
    public TMP_Text[] playerTimeTexts;

    [SerializeField]
    public TMP_Text[] playerNameTexts;

    public class Players
    {
        public string name;
        public float time;
    }

    //-------IMPORTANT, used to add time to the leaderboard--------\\
    public List<Players> playerList;

    public List<Players> GetPlayerList()
    {
        return playerList;
    }

    //--\\
    void Start()
    {
        playerList = new List<Players>();   //initiates the list
        // AddPlayersToList();   //fills the list with players
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
         
            // AddPlayersToList();
        }
       
        if (playerList.Count > 5)
        {
            Debug.Log("list has exceeded the limit");
            RemoveTime();
        }
    }
  

    //---------Sorts the list using inbuild List method----------\\

    void SortTheLeaderBoard()
    {
        playerList.Sort((x, y) => x.time.CompareTo(y.time));
        Debug.Log("LeaderBoard is now sorted");
    }


    //-----------Func for adding time to the list of players----------\\

    public void AddPlayersToList(float newTime, string newName)
    {
        Players timer = new Players { time = newTime };
        Players namer = new Players { name = newName };
        playerList.Add(timer);
        totalPlayers++;
        int rank = playerList.Count;
        Players player = new Players();
        player.time = Random.Range(0f, 100f);
        playerList.Add(player);

        SortTheLeaderBoard();

        for (int i = 0; i < Mathf.Min(playerTimeTexts.Length, playerList.Count); i++)
        {
            playerTimeTexts[i].SetText(playerList[i].time.ToString());
        }
        for (int i = 0; i < Mathf.Min(playerNameTexts.Length, playerList.Count); i++)
        {
            playerNameTexts[i].SetText(playerList[i].name); 
        }
    }

    public void RemoveTime()
    {
        playerList.RemoveAt(playerList.Count - 1);
    }
}
