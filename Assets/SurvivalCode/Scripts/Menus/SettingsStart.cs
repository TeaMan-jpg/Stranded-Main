using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsStart : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider SFXVolumeSlider;

    public float MouseSensitivity { get; private set; }

    private void Awake()
    {
        if (MusicVolumeSlider != null)
        {
            SetMusicVolume();
        }
       
        if (sensitivitySlider != null)
        {
            SetSensitivity();
        }
    }
   

    
    public void SetMusicVolume()
    {
        // Fix: Convert linear 0-1 slider value to logarithmic decibels (-80 to 0)
        // If volume is 0, we set it to -80db (silent)
        float volume = MusicVolumeSlider.value;
        Debug.Log("Music Volume: " + volume);
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);

    }

    public void SetSFXVolume()
    {
        // Fix: Convert linear 0-1 slider value to logarithmic decibels (-80 to 0)
        // If volume is 0, we set it to -80db (silent)
        float volume = SFXVolumeSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);

    }

    public void SetSensitivity()
    {
        MouseSensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat("SavedSensitivity", MouseSensitivity);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }


    
    // Call this in Start to make sure settings stay after closing the game
    
}
