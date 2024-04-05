using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Screen = UnityEngine.Device.Screen;

public class InputManager : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    
    public delegate void Tapped();
    public event Tapped OnTapped;

    public delegate void LeftSwipe();
    public event LeftSwipe OnSwipeLeft;
    public delegate void RightSwipe();
    public event RightSwipe OnSwipeRight;
    public delegate void UpSwipe();
    public event UpSwipe OnSwipeUp;
    public delegate void DownSwipe();
    public event DownSwipe OnSwipeDown;

    public delegate void KeyboardPressed(float turnInput);
    public event KeyboardPressed OnKeyboardPressed;
    
    public delegate void TouchDeltaX(float touchDeltaX);
    public event TouchDeltaX OnTouchDeltaX;

    public static event Action<InputActionMap> actionMapChange;
    #endregion

    [SerializeField] private float minimumDistance = 15f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;
    [SerializeField] public float screenEdgeThreshold = 0.1f;
    [HideInInspector] public float swipeDirection;

    private Vector2 startPosition, endPosition;
    private float startTime, endTime;
    
    private InputControls inputControls;
    private InputActionMap currentActionMap;

    private SimulatedPlayer simulatedPlayer;
    
    private bool isTouching;
    private Vector2 touchStart;
    private float touchDeltaNormalizedX;
    private Vector2 touchPosition;
    private Vector2 touchDelta;
    bool firstTap = true;

    private void Awake() {
        inputControls = new InputControls();
        
        inputControls.TouchControls.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        inputControls.TouchControls.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
        
        inputControls.TouchControls.SecondaryContact.started += ctx => StartTouchSecondary(ctx);
        inputControls.TouchControls.SecondaryContact.canceled += ctx => EndTouchSecondary(ctx);
        
        inputControls.TouchControls.Tap.performed += ctx => PerformedTap(ctx);

        inputControls.KeyboardControls.Move.performed += ctx => PerformedKeyboardPress(ctx);
        inputControls.KeyboardControls.Move.canceled += ctx => CanceledKeyboardPress(ctx);

        inputControls.KeyboardControls.Crouch.performed += ctx => OnSwipeDown();
        inputControls.KeyboardControls.Stand.performed += ctx => OnSwipeUp();
        
        simulatedPlayer = GetComponent<SimulatedPlayer>();
    }

    private void OnEnable() {
        inputControls.Enable();
        OnStartTouch += SwipeStart;
        OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        inputControls.Disable();
        OnStartTouch -= SwipeStart;
        OnEndTouch -= SwipeEnd;
    }
    private void Update()
    {
        if (isTouching)
        {
            CalculateHorizontalTouchDelta();
        }
        else
        {
            swipeDirection = 0;
            touchDeltaNormalizedX = 0f;
        }

        if (currentActionMap != (InputActionMap)inputControls.KeyboardControls) {
            simulatedPlayer.setTurnInput(touchDeltaNormalizedX);
        }
    }

    private void ToggleActionMap(InputActionMap actionMap) {
        if (actionMap.enabled && actionMap != currentActionMap) {
            inputControls.Disable();
            actionMapChange?.Invoke(actionMap);
            actionMap.Enable();
            currentActionMap = actionMap;
            // Debug.Log("Current action map toggled: " + currentActionMap.name);
        }
    } 

    private void StartTouchPrimary(InputAction.CallbackContext ctx) {
        isTouching = true;
        touchStart = inputControls.TouchControls.PrimaryPosition.ReadValue<Vector2>();
    }
    private void EndTouchPrimary(InputAction.CallbackContext ctx) {
        isTouching = false;
    }
    private void StartTouchSecondary(InputAction.CallbackContext ctx) {
        if (OnStartTouch != null) {
            OnStartTouch(ScreenPosition(), (float)ctx.startTime);
        }
    }
    private void EndTouchSecondary(InputAction.CallbackContext ctx) {
        if (OnEndTouch != null) {
            OnEndTouch(ScreenPosition(), (float)ctx.time);
        }
    }
    private void PerformedTap(InputAction.CallbackContext ctx) {
        if (OnTapped != null) {
            OnTapped();
        }
    }

    private void PerformedKeyboardPress(InputAction.CallbackContext ctx) {
        if (OnKeyboardPressed != null) {
            OnKeyboardPressed(ctx.ReadValue<float>());
        //    if (Application.platform == RuntimePlatform.WindowsPlayer) {
                ToggleActionMap(inputControls.KeyboardControls);
        //  }
        }
    }
    private void CanceledKeyboardPress(InputAction.CallbackContext ctx) {
        if (OnKeyboardPressed != null) {
            OnKeyboardPressed(ctx.ReadValue<float>());
        }
    }
    private void SwipeStart(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
        // if (Application.platform == RuntimePlatform.Android) {
            ToggleActionMap(inputControls.TouchControls); 
        // }
    }
    private void SwipeEnd(Vector2 position, float time) {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }
    
    private void DetectSwipe() {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance && 
            (endTime - startTime) < maximumTime) {
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }
    private void SwipeDirection(Vector2 direction) {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold) {
            if (OnSwipeUp != null) OnSwipeUp();
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold) {
            if (OnSwipeDown != null) OnSwipeDown();
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold) {
            if (OnSwipeLeft != null) OnSwipeLeft();
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold) {
            if (OnSwipeRight != null) OnSwipeRight();
        }
    }
    private Vector2 ScreenPosition() {
        return inputControls.TouchControls.SecondaryPosition.ReadValue<Vector2>();
    }
    
    private void CalculateHorizontalTouchDelta()
    {
        if (firstTap)
        {
            touchPosition = touchStart;
            touchStart = inputControls.TouchControls.PrimaryPosition.ReadValue<Vector2>();
            firstTap = false;
        }
        else
        {
            touchPosition = inputControls.TouchControls.PrimaryPosition.ReadValue<Vector2>();
        }
        touchDelta = touchPosition - touchStart;
        swipeDirection = touchDelta.x;
        
        // Debug.Log(touchDelta.x);
        
        //touchDelta = inputControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        
        float touchDeltaX = touchDelta.x/Screen.width * 2;
        
        touchDeltaNormalizedX = touchDeltaX /  (1 - screenEdgeThreshold);
        
        touchDeltaNormalizedX = Mathf.Clamp(touchDeltaNormalizedX, -1f, 1f);
        
        if (OnTouchDeltaX != null) {
            if (touchDeltaNormalizedX == 1 || touchDeltaNormalizedX == -1)
            {
                OnTouchDeltaX((1 - screenEdgeThreshold)*touchDeltaNormalizedX);
            }
            else
            {
                OnTouchDeltaX(touchDeltaX);
            }
        }
    }
}