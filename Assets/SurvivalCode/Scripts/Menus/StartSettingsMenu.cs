using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public string fallbackScene = "SampleScene";

    public void Play()
    {
        Platformers.SaveIO.Wipe();
        SceneManager.LoadScene(fallbackScene);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");

        //Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Persistence")));
    }

    public void OnDifficultyButtonClick(int choice)
    {
        // This creates the key "SelectedDifficulty" and saves the number choice
        PlayerPrefs.SetInt("SelectedDifficulty", choice);
        PlayerPrefs.Save(); // Saves it to the hard drive
    }

    public void LoadGame()
    {
        var save = Platformers.SaveIO.LoadOrNew();
        var sceneToLoad = string.IsNullOrEmpty(save.sceneName) ? fallbackScene : save.sceneName;
        SceneManager.LoadScene(sceneToLoad);
    }

    // Update is called once per frame
    void Update()
    {

    }
}