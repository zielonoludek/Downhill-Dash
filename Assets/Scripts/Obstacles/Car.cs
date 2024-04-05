using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Car : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 2f;
    SimulatedPlayer player;
    private void Awake()
    {
        player = FindFirstObjectByType<SimulatedPlayer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            player.SetPlayerVelocity(player.GetPlayerVelocity() * speedMultiplier);
        }
    }
}