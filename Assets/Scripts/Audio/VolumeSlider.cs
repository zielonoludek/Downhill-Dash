using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MENU,
        RACE,
        SFX
    }

    [Header("Type")]

    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumeType.MENU:
                volumeSlider.value = AudioManager.instance.menuVolume;
                break;
            case VolumeType.RACE:
                volumeSlider.value = AudioManager.instance.raceVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.instance.sfxVolume;
                break;
                    default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MENU:
                AudioManager.instance.menuVolume = volumeSlider.value;
                break;
            case VolumeType.RACE:
                AudioManager.instance.raceVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.instance.sfxVolume = volumeSlider.value;
                break;
                    default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }
}
