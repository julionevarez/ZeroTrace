using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ── SINGLETON ────────────────────────────────────────────────
    public static GameManager Instance;

    // ── GAME STATE ───────────────────────────────────────────────
    public int lootCollected = 0;
    public bool isGameOver = false;

    // ── UI REFERENCES ────────────────────────────────────────────
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI winStatsText;
    [SerializeField] private TextMeshProUGUI loseReasonText;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Awake()
    {
        // Set up singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    // Called by GuardDetection when player enters line of sight
    public void PlayerSpotted()
    {
        if (isGameOver) return;

        // Play MGS alert immediately then COD voice after delay
        AudioManager.Instance.PlayGuardAlert();
        AudioManager.Instance.PlayMissionFailed();
        LoseGame("You were spotted");
    }

    // Called when player reaches extraction zone with loot
    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Stop timer warning loop and play mission complete
        AudioManager.Instance.StopTimerWarning();
        AudioManager.Instance.PlayMissionComplete();

        // Show win panel with loot stats
        winStatsText.text = "Loot Collected: " + lootCollected;
        winPanel.SetActive(true);

        // Stop player movement
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) pc.canMove = false;

        Debug.Log("You extracted successfully!");
    }

    // Called when timer runs out or player is spotted
    public void LoseGame(string reason = "Timer ran out")
    {
        if (isGameOver) return;
        isGameOver = true;

        // Stop timer warning loop
        AudioManager.Instance.StopTimerWarning();

        // Show lose panel with reason
        loseReasonText.text = reason;
        losePanel.SetActive(true);

        // Stop player movement
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) pc.canMove = false;

        Debug.Log("Game over: " + reason);
    }

    // Called by restart buttons on win/lose screens
    public void RestartGame()
    {
        isGameOver = false;
        lootCollected = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}