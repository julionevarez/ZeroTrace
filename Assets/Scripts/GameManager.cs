using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ── SINGLETON ────────────────────────────────────────────────
    // Allows any script to access GameManager.Instance from anywhere
    public static GameManager Instance;

    // ── GAME STATE ───────────────────────────────────────────────
    public int lootCollected = 0;
    public bool isGameOver = false;

    // ── REFERENCES ───────────────────────────────────────────────
    [SerializeField] private GameObject losePanel;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Awake()
    {
        // Set up singleton — only one GameManager can exist
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

   // Called when player is spotted by a guard
public void PlayerSpotted()
{
    if (isGameOver) return;

    Debug.Log("Player spotted! Game over.");
    LoseGame();
}

    // Called when player reaches extraction with loot
    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("You extracted successfully!");
        // We'll add UI here in Week 2
    }

    // Called when timer runs out
    public void LoseGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("You lost!");
        // We'll add UI here in Week 2
    }

    // Restart the current scene
    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}