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
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    public void PlayerSpotted()
    {
        if (isGameOver) return;
        LoseGame("You were spotted");
    }

    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Show win panel with stats
        winStatsText.text = "Loot Collected: " + lootCollected;
        winPanel.SetActive(true);

        // Stop player movement
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) pc.canMove = false;

        Debug.Log("You extracted successfully!");
    }

    public void LoseGame(string reason = "Timer ran out")
    {
        if (isGameOver) return;
        isGameOver = true;

        // Show lose panel with reason
        loseReasonText.text = reason;
        losePanel.SetActive(true);

        // Stop player movement
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) pc.canMove = false;

        Debug.Log("Game over: " + reason);
    }

    public void RestartGame()
    {
        isGameOver = false;
        lootCollected = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}