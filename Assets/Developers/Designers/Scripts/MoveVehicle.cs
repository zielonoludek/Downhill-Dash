using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float moveSpeed = 5f; // Initial movement speed

    // Start is called before the first frame update
    void Start()
    {

        StartMoving();
    }

    // Method to move the object forward
    void StartMoving()
    {

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
