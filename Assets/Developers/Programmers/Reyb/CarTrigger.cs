using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrigger : MonoBehaviour
{
  public GameObject car;

  private void Awake()
  {
    car.SetActive(false);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("PlayerTrigger"))
    {
      car.SetActive(true);
    }
  }
}
