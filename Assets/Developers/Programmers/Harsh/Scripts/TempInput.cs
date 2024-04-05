using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempInput : MonoBehaviour
{
    PlayerTouchControls playerTouchControls;
    
    bool isTouching;
    Vector2 touchStart;
    Vector2 touchPosition;
    Vector2 touchDelta;
    float touchDeltaNormalizedX;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerTouchControls = new PlayerTouchControls();
        
        playerTouchControls.TouchMap.PrimaryTouch.started += ctx => TouchStarted(ctx);
        playerTouchControls.TouchMap.PrimaryTouch.canceled += ctx => TouchEnded(ctx);
        
    }
    
    void OnEnable()
    {
        playerTouchControls.Enable();
    }

    private void TouchEnded(InputAction.CallbackContext ctx)
    {
        isTouching = false;
    }

    private void TouchStarted(InputAction.CallbackContext ctx)
    {
        isTouching = true;
        touchStart = playerTouchControls.TouchMap.PrimaryPosition.ReadValue<Vector2>();
        //Debug.Log(touchStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouching)
        {
            CalculateHorizontalTouchDelta();
        }
        else
        {
            touchDeltaNormalizedX = 0f;
        }
        
        GetComponent<SimulatedPlayer>().setTurnInput(touchDeltaNormalizedX);
    }

    private void CalculateHorizontalTouchDelta()
    {
        touchPosition = playerTouchControls.TouchMap.PrimaryPosition.ReadValue<Vector2>();
        touchDelta = touchPosition - touchStart;
        
        float touchDeltaX = touchDelta.x;
        touchDeltaNormalizedX = touchDeltaX / Camera.main.pixelWidth;
        
        touchDeltaNormalizedX = Mathf.Clamp(touchDeltaNormalizedX, -1f, 1f);
        //Debug.Log(touchDeltaNormalizedX);
    }

    

    void OnDisable()
    {
        playerTouchControls.Disable();
    }
}
