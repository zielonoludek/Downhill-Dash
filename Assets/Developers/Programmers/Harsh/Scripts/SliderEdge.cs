using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderEdge : MonoBehaviour
{
    private InputManager inputManager;
    private Image sliderEdgeImage;
    private Vector3 touchStart;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        sliderEdgeImage = GetComponent<Image>();
        touchStart.y = sliderEdgeImage.rectTransform.anchoredPosition.y;
        sliderEdgeImage.enabled = false;
    }

    private void OnEnable() {
        inputManager = FindObjectOfType<InputManager>();
        inputManager.OnStartTouch += HandleStartTouch;
        inputManager.OnEndTouch += HandleEndTouch;
    }

    private void HandleEndTouch(Vector2 position, float time)
    {
        sliderEdgeImage.enabled = false;
    }

    private void HandleStartTouch(Vector2 position, float time)
    {
        sliderEdgeImage.enabled = true;
        
        float x = position.x;
        x = x/Screen.width;
        x *= 2;
        x -= 1;
        touchStart.x = x *400;
        
        
        sliderEdgeImage.rectTransform.localPosition = touchStart;
    }

    private void OnDisable() {
        inputManager.OnStartTouch -= HandleStartTouch;
        inputManager.OnEndTouch -= HandleEndTouch; }
}
