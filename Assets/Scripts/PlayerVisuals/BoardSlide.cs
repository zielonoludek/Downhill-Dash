using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlide : MonoBehaviour
{
    
    [SerializeField] private float standardAngleOffset = 15f;
    [SerializeField] private float maxAngleOffset = 30f;
    
    SimulatedPlayer simulatedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        simulatedPlayer = GetComponentInParent<SimulatedPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (simulatedPlayer.playerState == PlayerState.Sliding)
        {
            float angleOffset = (standardAngleOffset + simulatedPlayer.GetTurnInput()*standardAngleOffset);
            angleOffset = Mathf.Clamp(angleOffset, -maxAngleOffset, maxAngleOffset);
           Vector3 newLookDirection = Quaternion.AngleAxis(angleOffset, transform.up) * simulatedPlayer.transform.forward;
           transform.rotation = Quaternion.LookRotation(newLookDirection,transform.up);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,simulatedPlayer.transform.rotation,Time.deltaTime*10);
        }
    }
}
