using System;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class TouchInputSlider : MonoBehaviour
{
    private InputManager inputManager;
    private RectTransform sliderRectTransform;
    private Image sliderImage;
    private Canvas canvas;
    
    Vector2 touchStart;
    float rectWidth;
    
    private void Awake() {
        sliderRectTransform = GetComponent<RectTransform>();
        sliderImage = GetComponent<Image>();
        sliderImage.enabled = false;
        canvas = GetComponentInParent<Canvas>();
        touchStart.y = sliderRectTransform.anchoredPosition.y; 
    }

    private void Start()
    {
        rectWidth = 400;
    }

    private void OnEnable() {
        inputManager = FindObjectOfType<InputManager>();
        inputManager.OnStartTouch += HandleStartTouch;
        inputManager.OnEndTouch += HandleEndTouch;
        inputManager.OnTouchDeltaX += HandleTouchDeltaX;
    }

    private void OnDisable() {
        inputManager.OnStartTouch -= HandleStartTouch;
        inputManager.OnEndTouch -= HandleEndTouch;
        inputManager.OnTouchDeltaX -= HandleTouchDeltaX;
    }

    private void HandleStartTouch(Vector2 position, float time) {
        sliderImage.enabled = true; 
        float x = position.x;
        x = x/Screen.width;
        x *= 2;
        x -= 1;
        touchStart.x = x *rectWidth;
        
        sliderImage.rectTransform.localPosition = touchStart;
        
        //transform.position = new Vector2(position.x, position.y);
    }

    private void HandleEndTouch(Vector2 position, float time) {
        sliderImage.enabled = false;
    }

    private void HandleTouchDeltaX(float touchDeltaX ) 
    {
        //float scale = Screen.width / rectWidth;
        Vector2 newPosition = touchStart + new Vector2(touchDeltaX * rectWidth/2, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, -rectWidth, rectWidth);
        sliderImage.rectTransform.localPosition = newPosition;
    }
}
