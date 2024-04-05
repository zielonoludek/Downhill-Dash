using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    //public GameObject characterVisibility;
   // public  GameObject boardVisibility;
     public GameObject objectsToShow;

    private void Start()
    {
        Invoke("SetVisibility", 0f);
    }

    private void SetVisibility()
    {
        isVisible(objectsToShow);
    }

    private void isVisible(GameObject objectsToShow)
    {
        objectsToShow.SetActive(true);
    }
}