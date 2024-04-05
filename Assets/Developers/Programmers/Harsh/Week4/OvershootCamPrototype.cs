using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvershootCamPrototype : MonoBehaviour
{
    private SimulatedPlayer simulatedPlayer;
    
    private Transform currentTarget;
    
    [SerializeField] private float cameraYPosMin = 0.5f;
    [SerializeField] private float cameraYPosMax = 7.5f;
    [SerializeField] private float maxDistance  = 15;
    [SerializeField] private float minDistance  = 5;
    [SerializeField] private float minFOV = 60;
    [SerializeField] private float maxFOV = 90;
    [SerializeField] private float damping = 10;
    [SerializeField] private float rotationDamping = 10;
    
    [SerializeField] private float maxOvershootAngle = 15;

    private float turnRate;
    private float playerSpeed;
    private float minSpeed;
    private float maxSpeed;
    
    public float speedRatio;
    
    // Start is called before the first frame update
    void Awake()
    {
        simulatedPlayer = FindObjectOfType<SimulatedPlayer>();
        minSpeed = simulatedPlayer.GetMinSpeed();
        maxSpeed = simulatedPlayer.GetMaxSpeed();
        currentTarget = simulatedPlayer.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        turnRate = simulatedPlayer.GetTurnInput();
        playerSpeed = simulatedPlayer.GetPlayerSpeed();
        
        Vector3 directLookDirection = (currentTarget.transform.position - transform.position).normalized;
        Vector3 overshootDirection = Quaternion.AngleAxis(turnRate * maxOvershootAngle, Vector3.up) * directLookDirection;

        if (simulatedPlayer.IsGameStarted())
        {
            speedRatio = (playerSpeed - minSpeed) / (maxSpeed - minSpeed);
        }
        else
        {
            speedRatio = 0;
        }
        
        float cameraYPos = Mathf.Lerp(cameraYPosMax, cameraYPosMin, speedRatio);
        float distance = Mathf.Lerp(minDistance, maxDistance, speedRatio);
        
        Vector3 finalCameraPosition = currentTarget.transform.position - overshootDirection * distance;
        finalCameraPosition.y = currentTarget.transform.position.y + cameraYPos;
        
        transform.position = Vector3.Lerp(transform.position, finalCameraPosition, Time.deltaTime * damping);
        //transform.position = finalCameraPosition;
        Quaternion lookRotation = Quaternion.LookRotation(directLookDirection);
        //transform.rotation = lookRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 1000* rotationDamping);
        
        
        
        
        float fov = Mathf.Lerp(minFOV, maxFOV, speedRatio);
        Camera.main.fieldOfView = fov;
    }
    
    public void SwitchTargetToRagdoll(Transform ragdollTransform)
    {
        currentTarget = ragdollTransform;
    }
    
    public void SwitchTargetToPlayer()
    {
        currentTarget = simulatedPlayer.transform;
    }
}
