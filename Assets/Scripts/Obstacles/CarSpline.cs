using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpline : MonoBehaviour
{
    SimulatedPlayer player;
    private void Awake()
    {
        player = FindFirstObjectByType<SimulatedPlayer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger")) Destroy(transform.parent.gameObject);
    }
}
