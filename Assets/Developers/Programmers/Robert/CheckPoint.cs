using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isLatest = false;
    PlayerSpawnToCheckpoint playerScript;
    CameraCheckpoint cameraScript;

    void Start()
    {
        playerScript = FindAnyObjectByType<PlayerSpawnToCheckpoint>();
        //cameraScript = FindAnyObjectByType<CameraCheckpoint>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            // Set this checkpoint as the latest one
            isLatest = true;

            // Update respawn location only if this is the latest checkpoint
            if (isLatest)
            {
                //Debug.Log("Setting respawn location to " + transform.position);
                playerScript.respawnLocation = transform;
                //cameraScript.respawnLocation = transform;
            }

            // Reset isLatest flag for all other checkpoints
            CheckPoint[] checkpoints = FindObjectsOfType<CheckPoint>();
            foreach (CheckPoint checkpoint in checkpoints)
            {
                if (checkpoint != this)
                {
                    checkpoint.isLatest = false;
                }
            }
            
        }
    }
}
