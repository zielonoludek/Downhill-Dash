using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    [SerializeField] private float maxTrailWidth = 0.15f;
    [SerializeField] private float minTrailWidth = 0.05f;
    [SerializeField] private float maxTrailTime = 0.5f;
    [SerializeField] private float minTrailTime = 0.1f;
    
    private TrailRenderer trailRenderer;
    private SimulatedPlayer simulatedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        simulatedPlayer = GetComponent<SimulatedPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        float trailWidth = Mathf.Lerp(maxTrailWidth, minTrailWidth, simulatedPlayer.GetPlayerSpeed() / simulatedPlayer.GetMaxSpeed());
        float trailTime = Mathf.Lerp(maxTrailTime, minTrailTime, simulatedPlayer.GetPlayerSpeed() / simulatedPlayer.GetMaxSpeed());
        trailRenderer.widthMultiplier = trailWidth;
        trailRenderer.time = trailTime;
    }
}
