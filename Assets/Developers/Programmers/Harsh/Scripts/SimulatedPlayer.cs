using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;

public enum PlayerState
{
    Standing,
    Crouching,
    Sliding
}

public class SimulatedPlayer : MonoBehaviour
{

    public delegate void StateChange(PlayerState newState);
    public event StateChange OnStateChange;

    // slide button to grey out
    public GameObject slideButton;

    // Movement Config
    [Header("Forward Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float slideThresholdSpeed = 40f;
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float standingDrag = 0.0025f;
    [SerializeField] private float crouchingDrag = 0.005f;
    [SerializeField] private float slidingDrag = 0.01f;
    [SerializeField] private float slideEndSpeed = 30f;

    [Header("Gravity")]
    [SerializeField] private float slopeFactor = 1f;

    [Header("Turning")]
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float maxTurnAngle = 45f;
    [SerializeField] private float slideTurnFactor = 0.1f;
    [SerializeField] private float lateralFriction = 0.1f;
    [SerializeField] private float slideLateralFriction = 0.5f;
    [SerializeField] private float slideTime = 1.5f;


    // private variables
    private float gravity = -9.81f;
    private float currentSpeed;
    private float currentDrag;
    private Vector3 effectiveAcceleration;
    public PlayerState playerState;
    private PlayerState previousPlayerState;
    private float currentTurnAngle;
    private float turnInput;
    private Vector3 groundNormal;
    private bool isGrounded;
    private float currentLatFriction;
    private float slideTimer;

    private Vector3 currentVelocity;
    private bool bGameStarted = false;

    private Vector3 OriginalPlayerPosition;
    private Quaternion OriginalPlayerRotation;

    // Collision Config
    [Header("Collision")]
    [SerializeField] private float skinWidth = 0.1f;
    [SerializeField] private int collisionBounceDepth = 10;
    [SerializeField] private float boardOffset = 0.25f;

    // Components
    private Rigidbody kinematicRigidbody;
    private CapsuleCollider capsuleCollider;
    private AnimController animController;

    // input manager
    private InputManager inputManager;

    // game mode handler
    private GameModeHandler gameModeHandler;

    public float crashSpeed = 45f;

    private void Awake()
    {
        gameModeHandler = FindFirstObjectByType<GameModeHandler>();
        inputManager = GetComponent<InputManager>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        kinematicRigidbody = GetComponent<Rigidbody>();
        animController = FindObjectOfType<AnimController>();
    }

    private void OnEnable()
    {
        inputManager.OnSwipeDown += Crouch;
        inputManager.OnSwipeUp += Stand;
        inputManager.OnEndTouch += EndSlide;
        inputManager.OnKeyboardPressed += setTurnInput;

        //inputManager.OnSlide += Slide;

        gameModeHandler.OnRoundStart += StartGame;
        gameModeHandler.OnRoundEnd += StopGame;
        gameModeHandler.OnRoundTwoStart += StartGame;
        gameModeHandler.OnRoundTwoEnd += StopGame;
    }
    void Start()
    {
        OriginalPlayerRotation = transform.rotation;
        OriginalPlayerPosition = transform.position;
        currentLatFriction = lateralFriction;
        ChangePlayerState(PlayerState.Standing);
    }

    private void OnDisable()
    {
        inputManager.OnSwipeDown -= Crouch;
        inputManager.OnSwipeUp -= Stand;
        inputManager.OnEndTouch -= EndSlide;
        inputManager.OnKeyboardPressed -= setTurnInput;

        //inputManager.OnSlide -= Slide;

        gameModeHandler.OnRoundStart -= StartGame;
        gameModeHandler.OnRoundEnd -= StopGame;
        gameModeHandler.OnRoundTwoStart -= StartGame;
        gameModeHandler.OnRoundTwoEnd -= StopGame;
    }

    public void StartGame()
    {
        bGameStarted = true;
        currentVelocity = Vector3.zero;
        ChangePlayerState(PlayerState.Standing);
    }

    public void StopGame()
    {
        Debug.Log("StopGame");
        bGameStarted = false;
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        transform.position = OriginalPlayerPosition;
        transform.rotation = OriginalPlayerRotation;
        currentVelocity = Vector3.zero;
        ChangePlayerState(PlayerState.Standing);
    }

    void Update()
    {
        //Debug.Log(currentDrag);
        if (!bGameStarted) return;

        switch (playerState)
        {
            case PlayerState.Standing:
                {
                    if (currentSpeed > slideThresholdSpeed && (turnInput == 1 || turnInput == -1))
                    {
                        previousPlayerState = playerState;
                        ChangePlayerState(PlayerState.Sliding);
                    }
                    break;
                }
            case PlayerState.Crouching:
                {
                    if (currentSpeed > slideThresholdSpeed && (turnInput == 1 || turnInput == -1))
                    {
                        previousPlayerState = playerState;
                        ChangePlayerState(PlayerState.Sliding);
                    }
                    break;
                }
            case PlayerState.Sliding:
                {
                    if (currentSpeed < slideEndSpeed)
                    {
                        ChangePlayerState(previousPlayerState);
                        previousPlayerState = PlayerState.Sliding;
                    }
                    break;
                }
        }
        //turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        AlignToSlope();
        if (!bGameStarted) return;
        currentSpeed = currentVelocity.magnitude;
        if (playerState == PlayerState.Sliding)
        {
            currentDrag = slidingDrag * ((slideTimer / slideTime) * 0.75f + (Mathf.Abs(turnInput) * 0.25f));
            slideTimer += Time.fixedDeltaTime;
            if (slideTimer >= slideTime)
            {
                slideTimer = 0;
                previousPlayerState = playerState;
                ChangePlayerState(PlayerState.Standing);
            }
        }
        Turn();
        Move();
    }

    private void Turn()
    {
        if (playerState == PlayerState.Sliding)
        {
            currentTurnAngle = Mathf.Lerp(currentTurnAngle, (turnInput * maxTurnAngle * slideTurnFactor), 0.5f);
        }
        else
        {
            currentTurnAngle = Mathf.Lerp(currentTurnAngle, ((turnInput * maxTurnAngle) * 0.8f + (((maxSpeed - currentSpeed) / maxSpeed) * turnInput * maxTurnAngle) * 0.2f) / 2, 0.5f);
        }
        transform.Rotate(0, currentTurnAngle * Time.fixedDeltaTime * turnSpeed, 0);
    }

    void Move()
    {
        ApplyForces();
        transform.position += currentVelocity * Time.fixedDeltaTime;
    }

    private void ApplyForces()
    {
        /*
        Vector3 horizontalVelocity;
        horizontalVelocity = currentVelocity;
        effectiveAcceleration = transform.forward * acceleration;

        
        // Apply Lateral Friction
        Vector3 lateralVelocity = horizontalVelocity - Vector3.Project(horizontalVelocity, transform.forward);
        Vector3 lateralFrictionForce = -lateralVelocity.normalized * (currentLatFriction * lateralVelocity.magnitude * lateralVelocity.magnitude);
        effectiveAcceleration += lateralFrictionForce;
        
        
        
        effectiveAcceleration -= horizontalVelocity * (currentDrag * horizontalVelocity.magnitude);
        
        Debug.DrawRay(transform.position,Vector3.Cross(groundNormal, Vector3.Cross(Vector3.down, groundNormal)) *10f  , Color.red);
        
        horizontalVelocity += effectiveAcceleration * Time.fixedDeltaTime;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
        
        currentVelocity.x = horizontalVelocity.x;
        currentVelocity.z = horizontalVelocity.z;
        
        //currentVelocity = CollideAndSlide(currentVelocity,transform.position, 0, currentVelocity);
        
        Gravity();
        currentVelocity = CollideAndSlide(currentVelocity,transform.position, 0, currentVelocity);
        */

        float totalGravity = gravity * slopeFactor;
        if (isGrounded)
        {
            float slopeForwardComponent = Vector3.Dot(transform.forward, Vector3.Cross(groundNormal, Vector3.Cross(Vector3.down, groundNormal).normalized).normalized);
            float gravitation = totalGravity * slopeForwardComponent;

            Vector3 horizontalVelocity;
            horizontalVelocity = currentVelocity;
            effectiveAcceleration = transform.forward * (-gravitation + acceleration);

            // Apply Lateral Friction
            Vector3 lateralVelocity = horizontalVelocity - Vector3.Project(horizontalVelocity, transform.forward);
            Vector3 lateralFrictionForce = -lateralVelocity.normalized * (currentLatFriction * lateralVelocity.magnitude * lateralVelocity.magnitude);
            effectiveAcceleration += lateralFrictionForce;

            effectiveAcceleration -= horizontalVelocity * (currentDrag * horizontalVelocity.magnitude);

            horizontalVelocity += effectiveAcceleration * Time.fixedDeltaTime;
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);

            currentVelocity = horizontalVelocity;
        }
        else
        {
            Gravity();
        }

        currentVelocity = CollideAndSlide(currentVelocity, transform.position, 0, currentVelocity);
    }

    void Gravity()
    {
        currentVelocity.y += gravity * Time.fixedDeltaTime * slopeFactor;
    }

    public void ChangePlayerState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Standing:
                currentDrag = standingDrag;
                currentLatFriction = lateralFriction;
                slideTimer = 0;
                break;
            case PlayerState.Crouching:
                currentDrag = crouchingDrag;
                currentLatFriction = lateralFriction;
                slideTimer = 0;
                break;
            case PlayerState.Sliding:
                currentDrag = slidingDrag;
                currentLatFriction = slideLateralFriction;
                break;
        }

        playerState = newState;

        if (OnStateChange != null)
        {
            OnStateChange(newState);
        }
    }

    void AlignToSlope()
    {
        //Quaternion currentRotation = transform.rotation;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, capsuleCollider.height * (0.5f + 2 * boardOffset), LayerMask.GetMask("Ground")))
        {
            Quaternion targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.fixedDeltaTime);
            transform.rotation = targetRotation;
            transform.position = hit.point + transform.up * (capsuleCollider.height * (0.5f + boardOffset));
            groundNormal = hit.normal;
            isGrounded = true;
        }
        else
        {
            Quaternion targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, Time.fixedDeltaTime);
            transform.rotation = targetRotation;
            groundNormal = Vector3.up;
            isGrounded = false;
        }
        //Debug.DrawLine(transform.position, -transform.up * capsuleCollider .height *0.65f + transform.position, Color.blue);
    }

    Vector3 CollideAndSlide(Vector3 velocity, Vector3 position, int depth, Vector3 initialVelocity)
    {
        //Debug.DrawRay(position, velocity, Color.green);
        if (depth >= collisionBounceDepth)
        {
            return Vector3.zero;
        }

        Bounds bounds = capsuleCollider.bounds;

        bounds.Expand(skinWidth * -2f);

        float distance = (velocity.magnitude) * Time.fixedDeltaTime + skinWidth;

        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center - Vector3.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius);

        RaycastHit hit;
        if (Physics.CapsuleCast(point1, point2, capsuleCollider.radius, velocity.normalized, out hit, distance, LayerMask.GetMask("Ground")))
        {
            if (hit.collider.CompareTag("Crash") && currentSpeed > crashSpeed)
            {
                /*
                GetComponent<PlayerSpawnToCheckpoint>().respawn = true;
                return Vector3.zero;
                */
                
                bGameStarted = false;
                GetComponentInChildren<Ragdoll>().EnableRagdoll();
                currentVelocity = Vector3.zero;
                
                return Vector3.zero;
             
            }
            else if (hit.collider.CompareTag("Crash") && currentSpeed < crashSpeed)
            {
                //velocity = velocity/2;
            }
           
            //Debug.Log("Hit");
            Vector3 snapToSurface = velocity.normalized * (hit.distance - skinWidth);
            Vector3 leftOverVelocity = velocity - snapToSurface;

            if (snapToSurface.magnitude <= skinWidth)
            {
                snapToSurface = Vector3.zero;
            }

            float scale = Vector3.Dot(leftOverVelocity.normalized, initialVelocity.normalized);

            float magnitude = leftOverVelocity.magnitude;
            leftOverVelocity = Vector3.ProjectOnPlane(leftOverVelocity, hit.normal).normalized;
            //leftOverVelocity *= magnitude;
            leftOverVelocity *= magnitude * scale;

            return snapToSurface + CollideAndSlide(leftOverVelocity, position + snapToSurface, depth + 1, initialVelocity);
        }


        //Debug.Log(depth);
        //Debug.DrawRay(position, velocity, Color.blue);
        return velocity;
    }

    public void Stand()
    {
        previousPlayerState = playerState;
        ChangePlayerState(PlayerState.Standing);
    }

    public void Crouch()
    {
        previousPlayerState = playerState;
        ChangePlayerState(PlayerState.Crouching);
    }

    public void Slide()
    {
        if (currentSpeed > slideThresholdSpeed)
        {
            previousPlayerState = playerState;
            ChangePlayerState(PlayerState.Sliding);

        }
    }

    private void EndSlide(Vector2 position, float time)
    {
        if (playerState == PlayerState.Sliding)
        {
            ChangePlayerState(previousPlayerState);
            previousPlayerState = PlayerState.Sliding;
        }
    }

    public void setTurnInput(float turnInput)
    {
        this.turnInput = turnInput;
    }

    public float GetPlayerSpeed()
    {
        return currentSpeed;
    }

    public void SetPlayerVelocity(Vector3 velocity)
    {
        currentVelocity = velocity;
    }

    public Vector3 GetPlayerVelocity()
    {
        return currentVelocity;
    }

    public float GetTurnInput()
    {
        return turnInput;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetMinSpeed()
    {
        return minSpeed;
    }
    public string GetState()
    {
        return playerState.ToString();
    }
    
    public bool IsGameStarted()
    {
        return bGameStarted;
    }
}
