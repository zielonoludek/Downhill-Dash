using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLean : MonoBehaviour
{
    [SerializeField] private float leanFactor = 15f;
    
    SimulatedPlayer simulatedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        simulatedPlayer = GetComponentInParent<SimulatedPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newUpDirection = Quaternion.AngleAxis(-simulatedPlayer.GetTurnInput()*leanFactor, transform.forward) * simulatedPlayer.transform.up;
        transform.rotation = Quaternion.LookRotation(transform.forward,newUpDirection);
    }
}
