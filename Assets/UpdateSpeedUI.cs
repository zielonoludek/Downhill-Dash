using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateSpeedUI : MonoBehaviour
{
    // Start is called before the first frame update
    
    TextMeshProUGUI speedText;
    SimulatedPlayer player;
    void Start()
    {
        player = FindObjectOfType<SimulatedPlayer>();
        speedText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = "Speed: " + player.GetPlayerSpeed().ToString("F0");
    }
}
