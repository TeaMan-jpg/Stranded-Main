using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerStart : MonoBehaviour
{
    // Start is called before the first frame update



    // Start is called before the first frame update
    [SerializeField] AudioSource musicSource;

    public AudioClip background;
 


    public void Start()
    {

        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }



}
