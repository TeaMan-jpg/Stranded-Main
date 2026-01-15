using Platformers;
using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [SerializeField] private DifficultySettings easySettings;
    [SerializeField] private DifficultySettings mediumSettings;
    [SerializeField] private DifficultySettings hardSettings;

    public DifficultySettings currentDifficulty;
    public DifficultyLevel CurrentLevel { get; private set; }

    // Use a constant string to avoid typos
    private const string PrefKey = "SelectedDifficulty";

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            //Destroy(gameObject);
            return;
        }
        Instance = this;

        // --- THE FIX ---
        // 1. Don't hardcode SetDifficulty(0). 
        // 2. Instead, ask PlayerPrefs what was picked in the last scene.
        // Default to 1 (Medium) if nothing was picked yet.
        int savedLevel = PlayerPrefs.GetInt(PrefKey, 1);

        // This sets the Level and the Settings
        ApplyDifficultyInternal(savedLevel);
    }

    public void SetDifficulty(int level)
    {
        // 1. Save the choice to the computer so the next scene can see it
        PlayerPrefs.SetInt(PrefKey, level);
        PlayerPrefs.Save();

        // 2. Update the current instance immediately
        ApplyDifficultyInternal(level);

        Debug.Log("Difficulty saved and set to: " + CurrentLevel);
    }

    // Helper to set both the Enum and the ScriptableObject Reference
    private void ApplyDifficultyInternal(int level)
    {
        if (level == 0) CurrentLevel = DifficultyLevel.Easy;
        else if (level == 1) CurrentLevel = DifficultyLevel.Medium;
        else if (level == 2) CurrentLevel = DifficultyLevel.Hard;

        SetSettings();
    }

    private void SetSettings()
    {
        switch (CurrentLevel)
        {
            case DifficultyLevel.Easy:
                currentDifficulty = easySettings;
                break;
            case DifficultyLevel.Medium:
                currentDifficulty = mediumSettings;
                break;
            case DifficultyLevel.Hard:
                currentDifficulty = hardSettings;
                break;
        }
    }
}