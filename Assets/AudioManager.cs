using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip slash;
    public AudioClip block;
    public AudioClip enemyDeath;
    public AudioClip background;
    [SerializeField] private SettingsMenu settingsMenu;

    public void Start()
    {
        
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

   

    public void PlaySFX(string clip)
    {
        switch (clip)
        {
            case "slash":
                sfxSource.PlayOneShot(slash);
                break;
            case "block":
                sfxSource.PlayOneShot(block);
                break;
            case "enemyDeath":
                sfxSource.PlayOneShot(enemyDeath);
                break;
            default:
                Debug.Log("No SFX found");
                break;
        }
    }
}
