using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheckpoint : MonoBehaviour
{
    public Transform respawnLocation;

    public void Respawn()
    {
        transform.position = respawnLocation.position;
    }
}
