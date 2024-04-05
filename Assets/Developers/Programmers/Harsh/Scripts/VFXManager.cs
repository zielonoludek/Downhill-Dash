using UnityEngine;

public class VFXManager : MonoBehaviour
{
    SimulatedPlayer player;
    
    ParticleSystem slideVFX;
    ParticleSystem speedVFX;
    private ParticleSystem[] particles;
    
    private void Awake()
    {
        player = GetComponentInParent<SimulatedPlayer>();
        particles = GetComponentsInChildren<ParticleSystem>();


        slideVFX = particles[0];
        speedVFX = particles[1];
        
        slideVFX.Stop();
        speedVFX.Stop();
    }

    private void OnEnable()
    {
        player.OnStateChange += HandleStateChanged;
    }
    
    private void OnDisable()
    {
        player.OnStateChange -= HandleStateChanged;
    }
    
    void HandleStateChanged(PlayerState newState)
    {
        if(newState == PlayerState.Sliding) slideVFX.Play();
        else if (newState == PlayerState.Crouching) speedVFX.Play();
        else
        {
            slideVFX.Stop();
            speedVFX.Stop();
        }
    }
}
