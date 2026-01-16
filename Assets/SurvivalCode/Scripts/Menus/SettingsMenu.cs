using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro; // Recommended to use TextMeshPro

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider SFXVolumeSlider;

    Resolution[] resolutions;

    // We make this 'static' so your Player/Camera script can access it easily
    public static float MouseSensitivity = 1f;

    private void Awake()
    {
        if (MusicVolumeSlider != null)
        {
            SetMusicVolume();
        }
        if (SFXVolumeSlider != null)
        {
            SetSFXVolume();
        }
        if (sensitivitySlider != null)
        {
            SetSensitivity();
        }
    }
    void Start()
    {
       
        LoadSettings();
        
    }

    

    public void SetMusicVolume()
    {
        // Fix: Convert linear 0-1 slider value to logarithmic decibels (-80 to 0)
        // If volume is 0, we set it to -80db (silent)
        float volume = MusicVolumeSlider.value;
        Debug.Log("Music Volume: " + volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

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

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Call this in Start to make sure settings stay after closing the game
    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SavedVolume"))
        {
            float vol = PlayerPrefs.GetFloat("SavedVolume");
            MusicVolumeSlider.value = vol;
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SavedSensitivity"))
        {
            float sens = PlayerPrefs.GetFloat("SavedSensitivity");
            sensitivitySlider.value = sens;
            MouseSensitivity = sens;
        }
    }
}