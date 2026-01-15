using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");

        //Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Persistence")));
    }

    public void OnDifficultyButtonClick(int choice)
    {
        // This creates the key "SelectedDifficulty" and saves the number choice
        PlayerPrefs.SetInt("SelectedDifficulty", choice);
        PlayerPrefs.Save(); // Saves it to the hard drive
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
