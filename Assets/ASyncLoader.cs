using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 1. Add this namespace to use the modern Keyboard library
using UnityEngine.InputSystem;

public class ASyncLoader : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider slider;

    [Header("Settings")]
    [SerializeField] private string levelToLoad;

    private bool isLoading = false;

    void Update()
    {
        // 2. Use Keyboard.current to detect a key press (e.g., Space bar)
        // Ensure Keyboard.current is not null (case for some builds or non-keyboard devices)
        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame && !isLoading)
        {
            LoadLevelBtn(levelToLoad);
        }
    }

    public void LoadLevelBtn(string levelName)
    {
        if (isLoading) return;

        isLoading = true;
        loadingScreen.SetActive(true);
        canvas.SetActive(false); // Hide the main menu UI

        StartCoroutine(LoadLevelAsync(levelName));
    }

    public IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}