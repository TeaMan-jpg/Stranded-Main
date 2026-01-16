using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI References (drag your TMP texts here)")]
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text timerText;

    [Header("Starting Values")]
    [SerializeField] private int startingGold = 0;
    [SerializeField] private float levelTimeSeconds = 120f;

    [Header("Timer")]
    [SerializeField] private bool startTimerOnPlay = true;

    public int Gold => _gold;
    public float TimeRemaining => _timeRemaining;
    public bool TimerRunning => _timerRunning;

    private int _gold;
    private float _timeRemaining;
    private bool _timerRunning;

    private void Awake()
    {
        _gold = Mathf.Max(0, startingGold);
        _timeRemaining = Mathf.Max(0f, levelTimeSeconds);
        _timerRunning = startTimerOnPlay;

        RefreshGoldUI();
        RefreshTimerUI();
    }

    private void Update()
    {
        if (!_timerRunning) return;

        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            _timerRunning = false;
        }

        RefreshTimerUI();
    }

    public void AddGold(int amount)
    {
        if (amount == 0) return;

        _gold = Mathf.Max(0, _gold + amount);
        RefreshGoldUI();
    }

    public bool TrySpendGold(int amount)
    {
        if (amount <= 0) return true;
        if (_gold < amount) return false;

        _gold -= amount;
        RefreshGoldUI();
        return true;
    }

    public void SetGold(int newGold)
    {
        _gold = Mathf.Max(0, newGold);
        RefreshGoldUI();
    }

    public void StartTimer()
    {
        _timerRunning = true;
    }

    public void StopTimer()
    {
        _timerRunning = false;
    }

    public void ResetTimer(float seconds)
    {
        _timeRemaining = Mathf.Max(0f, seconds);
        RefreshTimerUI();
    }

    public void AddTime(float seconds)
    {
        _timeRemaining = Mathf.Max(0f, _timeRemaining + seconds);
        RefreshTimerUI();
    }

    private void RefreshGoldUI()
    {
        if (goldText == null) return;
        goldText.text = $"Gold: {_gold}";
    }

    private void RefreshTimerUI()
    {
        if (timerText == null) return;

        int mins = Mathf.FloorToInt(_timeRemaining / 60f);
        int secs = Mathf.FloorToInt(_timeRemaining % 60f);

        timerText.text = $"{mins:00}:{secs:00}";
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (startingGold < 0) startingGold = 0;
        if (levelTimeSeconds < 0f) levelTimeSeconds = 0f;
    }
#endif
}
