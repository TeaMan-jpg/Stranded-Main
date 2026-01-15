using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene1");

    }



    public void Quit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    
}
