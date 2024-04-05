using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstructionCulling : MonoBehaviour
{
    private GameObject[] currentlyObstructed;
    
    private SimulatedPlayer simulatedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        simulatedPlayer = FindObjectOfType<SimulatedPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // first clear the currentlyObstructed array
        if (currentlyObstructed != null)
        {
            foreach (GameObject go in currentlyObstructed)
            {
                if (go.CompareTag("ground"))
                {
                    go.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
        
        if (simulatedPlayer != null)
        {
            Vector3 direction = simulatedPlayer.transform.position - transform.position;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, direction.normalized, direction.magnitude);
            currentlyObstructed = hits.Select(hit => hit.collider.gameObject).ToArray();
            foreach (GameObject go in currentlyObstructed)
            {
                if (go.CompareTag("ground"))
                {
                    go.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }
        }
    }
}
