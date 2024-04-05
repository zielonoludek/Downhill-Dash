using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]

    public float masterVolume = 1;
    [Range(0, 1)]

    public float menuVolume = 1;
    [Range(0, 1)]

    public float raceVolume = 1;
    [Range(0, 1)]

    public float sfxVolume = 1;
    [Range(0, 1)]


    private Bus masterBus;

    private Bus menuBus;

    private Bus raceBus;

    private Bus sfxBus;

    public static AudioManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;


        masterBus = RuntimeManager.GetBus("bus:/");
        menuBus = RuntimeManager.GetBus("bus:/Menu");
        raceBus = RuntimeManager.GetBus("bus:/Race");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        menuBus.setVolume(menuVolume);
        raceBus.setVolume(raceVolume);
        sfxBus.setVolume(sfxVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
