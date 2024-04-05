using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //----power slide to make


    //---------------------Adjustable LongBoard physics Variables-----------------------\\

    [Header("Speed Variables")]

    [SerializeField]
    float forwardSpeed;

    [SerializeField]
    float steeringSpeed;

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    float increasingSpeed;

    [SerializeField]
    float slowingSpeed;

    [SerializeField]
    float jumpHeight;
    

    //---\\

    public Rigidbody longBoard;
    public bool isOnGround;


    //----------------------Player state and Update----------------------\\


    public PlayerState currentPlayerState = PlayerState.getNormalSpeed;


    public enum PlayerState
    {
        getNormalSpeed,
        speedUp,
        speedDown,
        powerSlide,
    }

    void UpdateState()
    {
        switch (currentPlayerState)
        {
            case PlayerState.getNormalSpeed: GetNormalVelocity(); break;
                case PlayerState.speedUp: VelocityUp(); break;
                    case PlayerState.speedDown: VelocityDown(); break;
                        case PlayerState.powerSlide: PowerSlide(); break;
                            default: break;
        }
    }

    void FixedUpdate()
    {

        Inputs();
        UpdateState();
        Jump();
        RotationBoard();
        longBoard.transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);


    }

    //-------------------------Logic for rotation towards direction---------------------------\\
    public void RotationBoard()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movementDirection = new Vector3(horizontalInput, 0, 0);
        movementDirection.Normalize();
        transform.Translate(movementDirection * steeringSpeed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }



    //-------------------------Inputs---------------------------\\
    public void Inputs()
    {
        if (Input.GetKey(KeyCode.S))
        {
            currentPlayerState = PlayerState.speedDown;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            currentPlayerState = PlayerState.speedUp;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            currentPlayerState = PlayerState.powerSlide;
        }
     
        else
        {
            currentPlayerState = PlayerState.getNormalSpeed;
        }
    }


     //-------------------------Fake Acceleration Func---------------------------\\
    public void GetNormalVelocity()
    {
        forwardSpeed = Mathf.Lerp(forwardSpeed, 40, 0.01f);
        steeringSpeed = 5;
        rotationSpeed = 10;
       
    }

    public void VelocityDown()
    {
        forwardSpeed = Mathf.Lerp(forwardSpeed, slowingSpeed, 0.01f);
    }

    public void VelocityUp()
    {
        forwardSpeed = Mathf.Lerp(forwardSpeed, increasingSpeed, 0.05f);
    }
    public void PowerSlide()
    {
        forwardSpeed = Mathf.Lerp(forwardSpeed, slowingSpeed, 0.3f);
        steeringSpeed = Mathf.Lerp(steeringSpeed, 20, 0.3f);
        rotationSpeed = Mathf.Lerp(rotationSpeed, 70, 0.5f);
    }


    //--------------------Jump--------------------\\

    public void Jump()
    {
        if(isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            longBoard.AddForce(Vector3.up * jumpHeight);
        }
    }


    //---------------------Coliders------------------------\\

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            isOnGround = true;
        }
    }

}
