using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class CarInitialOffset : MonoBehaviour
{
    [SerializeField] public SplineContainer spline;
    private SplineAnimate splineAnimate;
    // Start is called before the first frame update
    void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();

        if (spline!=null)
        {
            Debug.Log("Spline found");
            SplineUtility.GetNearestPoint(spline.Spline, transform.position, out float3 point, out float t, 64,64);
            splineAnimate.StartOffset = t;
            Debug.Log("StartOffset: " + splineAnimate.StartOffset);
        }
    }
}
