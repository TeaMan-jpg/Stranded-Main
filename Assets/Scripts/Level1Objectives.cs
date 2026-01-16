using System.Collections;
using TMPro;
using UnityEngine;

public class Level1Objectives : MonoBehaviour
{
    public static Level1Objectives Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text objectiveText; // top-right
    [SerializeField] private TMP_Text popupText;     // center

    [Header("Objective")]
    [SerializeField] private int totalChests = 16;

    [Header("Big Chest (center of map)")]
    [SerializeField] private GameObject bigChest;

    [Header("Popup")]
    [SerializeField] private float popupSeconds = 4f;
    [SerializeField] private KeyCode dismissKey = KeyCode.E;
    [SerializeField] private float dismissLockSeconds = 0.15f;

    public int OpenedChests { get; private set; }

    private bool completed;
    private bool popupVisible;
    private Coroutine popupRoutine;
    private float popupShownTime;

    private void Awake()
    {
        Instance = this;

        if (popupText != null)
            popupText.gameObject.SetActive(false);

        if (bigChest != null)
            bigChest.SetActive(false);

        UpdateObjectiveUI();
    }

    private void Update()
    {
        if (popupVisible &&
            Input.GetKeyDown(dismissKey) &&
            Time.time - popupShownTime > dismissLockSeconds)
        {
            HidePopup();
        }
    }

    public void ChestOpened()
    {
        if (completed) return;

        OpenedChests++;
        if (OpenedChests > totalChests) OpenedChests = totalChests;

        if (OpenedChests >= totalChests)
        {
            completed = true;

            // Top-right completed message
            if (objectiveText != null)
                objectiveText.text = "Objective complete!";

            // Show big chest
            if (bigChest != null)
                bigChest.SetActive(true);

            // Popup
            ShowPopup("All chests found!\nGo to the center of the map!\n\nPress E to dismiss", popupSeconds);

            return;
        }

        UpdateObjectiveUI();
    }

    private void UpdateObjectiveUI()
    {
        if (objectiveText == null) return;
        objectiveText.text = $"Chests: {OpenedChests}/{totalChests}";
    }

    public void ShowPopupMessage(string message, float seconds)
    {
        ShowPopup(message, seconds);
    }

    private void ShowPopup(string message, float seconds)
    {
        if (popupText == null) return;

        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupText.text = message;
        popupText.gameObject.SetActive(true);

        popupVisible = true;
        popupShownTime = Time.time;

        if (seconds > 0f)
            popupRoutine = StartCoroutine(PopupTimer(seconds));
    }

    private IEnumerator PopupTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePopup();
        popupRoutine = null;
    }

    private void HidePopup()
    {
        if (popupText != null)
            popupText.gameObject.SetActive(false);

        popupVisible = false;

        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
            popupRoutine = null;
        }
    }
}
