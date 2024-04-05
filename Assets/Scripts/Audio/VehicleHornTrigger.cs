using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleHornTrigger : MonoBehaviour
{



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.closeVehicle, this.transform.position);
        }




    }
}
