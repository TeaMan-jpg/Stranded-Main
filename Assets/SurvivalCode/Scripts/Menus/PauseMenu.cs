using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Platformers;

//public class PauseMenu : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private GameObject menuCanvas;
//    [SerializeField] private FirstPersonController playerController;

//    [Header("Input Settings")]
//    [SerializeField] private InputActionAsset inputActions; // Drag the .inputactions file here

//    // String names MUST match your Input Action Asset exactly
//    [SerializeField] private string actionMapName = "Pause"; // or "Pause" if you made a new map
//    [SerializeField] private string actionName = "Pause";

//    private InputAction pauseAction;
//    private bool isPaused = false;

//    private void Awake()
//    {
//        // 1. Validate References
//        if (inputActions == null)
//        {
//            Debug.LogError("CRITICAL: Input Action Asset is missing in Inspector!", this);
//            enabled = false; // Stop script to prevent errors
//            return;
//        }

//        // 2. Find the action
//        // Note: FindActionMap throws an error if the Map name is wrong.
//        var map = inputActions.FindActionMap("PauseMap");
//        if (map == null)
//        {
//            Debug.LogError($"Could not find Action Map named '{actionMapName}'");
//            return;
//        }

//        pauseAction = map.FindAction(actionName);
//        if (pauseAction == null)
//        {
//            Debug.LogError($"Could not find Action named '{actionName}' inside map '{actionMapName}'");
//        }
//    }

//    private void Start()
//    {
//        // Manual assignment is safer than Find()
//        if (menuCanvas == null) Debug.LogError("Assign Menu Canvas in Inspector!");
//        else menuCanvas.SetActive(false);
//    }

//    public void OnEnable()
//    {
//        if (pauseAction != null) pauseAction.Enable();
//    }

//    public void OnDisable()
//    {
//        if (pauseAction != null) pauseAction.Disable();
//    }

//    void Update()
//    {
//        // 3. USE .triggered INSTEAD OF .ReadValue
//        // .triggered is the equivalent of .wasPressedThisFrame
//        if (pauseAction != null && pauseAction.triggered)
//        {
//            Debug.Log("Pause Button Detected!");

//            if (isPaused)
//                ResumeGame();
//            else
//                PauseGame();
//        }
//    }

//    public void PauseGame()
//    {
//        menuCanvas.SetActive(true);
//        Time.timeScale = 0f; // Freeze time
//        if (playerController != null) playerController.SetControlsEnabled(false);
//        isPaused = true;
//    }

//    public void ResumeGame()
//    {
//        menuCanvas.SetActive(false);
//        Time.timeScale = 1f; // Resume time
//        if (playerController != null) playerController.SetControlsEnabled(true);
//        isPaused = false;
//    }

//    public void QuitGame()
//    {
//        Application.Quit();
//        Debug.Log("Quitting...");
//    }
//}
namespace Platformers
{
    public class PauseMenu : MonoBehaviour
    {
        //[SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private FirstPersonController player;

        

        public void Quit()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menus");

            Debug.Log("Loading Menus...");
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            //pauseMenuUI.SetActive(false);
            player.SetControlsEnabled(true);
            Debug.Log("Resuming Game...");
            player.CloseAllMenus();
        }

        public void Save()
        {
            //player.SavePlayer();
            Debug.Log("Game Saved!");
        }
    }
}