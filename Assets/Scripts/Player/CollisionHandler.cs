using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float thresholdSpeed = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crash") && GetComponentInParent<SimulatedPlayer>().GetPlayerSpeed() > thresholdSpeed)
        {
            GetComponentInParent<PlayerSpawnToCheckpoint>().respawn = true;
            FindAnyObjectByType<CameraCheckpoint>().Respawn();
        }
    }
}
