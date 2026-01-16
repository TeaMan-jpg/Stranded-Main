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
    public AudioClip hit;
    public AudioClip walking;
    public AudioClip running;
    public AudioClip slowsteps;
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] AudioSource footstepSource; 


    public void Start()
    {
        
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }



    public void PlaySFX(string clipName)
    {
        AudioClip targetClip = null;

        switch (clipName)
        {
            case "slash": targetClip = slash; break;
            case "block": targetClip = block; break;
            case "enemyDeath": targetClip = enemyDeath; break;
            
                // ... add others
        }

        if (targetClip != null)
        {
            // Add variety so it doesn't sound robotic
            sfxSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(targetClip);
        }
    }
    public void PlayFootstep(string clipName)
    {
        AudioClip target = null;
        if (clipName == "walking") target = walking;
        if (clipName == "running") target = running;
        if (clipName == "slowsteps") target = slowsteps;

        if (target != null)
        {
            footstepSource.clip = target;
            footstepSource.pitch = Random.Range(0.8f, 1.1f);
            footstepSource.Play();
        }
    }

    public void StopFootstep()
    {
        footstepSource.Stop();
    }
}
