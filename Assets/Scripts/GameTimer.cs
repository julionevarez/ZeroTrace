using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private float totalTime = 90f;

    // ── UI REFERENCES ────────────────────────────────────────────
    [SerializeField] private TextMeshProUGUI timerLabel;

    // ── MGS-STYLE COLORS ─────────────────────────────────────────
    private Color greenColor  = new Color(0f, 1f, 0f, 1f);
    private Color orangeColor = new Color(1f, 0.5f, 0f, 1f);
    private Color redColor    = new Color(1f, 0f, 0f, 1f);

    // ── PRIVATE STATE ────────────────────────────────────────────
    private float timeRemaining;
    private bool isRunning = true;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        timeRemaining = totalTime;
        timerLabel.color = greenColor;
    }

    void Update()
    {
        if (!isRunning || GameManager.Instance.isGameOver) return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0f);

        float fillAmount = timeRemaining / totalTime;
        int seconds = Mathf.CeilToInt(timeRemaining);

        // MGS-style color progression
        if (fillAmount > 0.5f)
        {
            // Green — plenty of time
            timerLabel.color = greenColor;
            timerLabel.text = seconds + "s";
        }
        else if (fillAmount > 0.25f)
        {
            // Orange — getting low
            timerLabel.color = orangeColor;
            timerLabel.text = seconds + "s";
        }
        else
        {
            // Red + flashing — critical
            float flash = Mathf.PingPong(Time.time * 4f, 1f);
            timerLabel.color = new Color(1f, flash * 0.3f, 0f, 1f);
            timerLabel.text = "!! " + seconds + "s !!";
        }

        // Time ran out
        if (timeRemaining <= 0f)
        {
            isRunning = false;
            GameManager.Instance.LoseGame();
        }
    }

    // Pause/resume timer
    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}