using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootMinigame : MonoBehaviour
{
    // ── SINGLETON ────────────────────────────────────────────────
    public static LootMinigame Instance;

    // ── UI REFERENCES ────────────────────────────────────────────
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TextMeshProUGUI promptText;

    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private int sequenceLength = 4;

    // ── PRIVATE STATE ────────────────────────────────────────────
    private KeyCode[] sequence;
    private int currentIndex = 0;
    private bool isActive = false;
    private LootContainer currentContainer;

    // Arrow key display symbols
    private string GetArrowSymbol(KeyCode key)
{
    switch (key)
    {
        case KeyCode.UpArrow:    return "⬆";
        case KeyCode.DownArrow:  return "⬇";
        case KeyCode.LeftArrow:  return "⬅";
        case KeyCode.RightArrow: return "➡";
        default: return "?";
    }
}

    // ── UNITY METHODS ────────────────────────────────────────────

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!isActive) return;

        // Check for arrow key input
        KeyCode[] arrowKeys = {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };

        foreach (KeyCode key in arrowKeys)
        {
            if (Input.GetKeyDown(key))
            {
                HandleInput(key);
                break;
            }
        }
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    // Called by LootContainer when player presses E
    public void StartMinigame(LootContainer container)
    {
        currentContainer = container;
        currentIndex = 0;
        isActive = true;

        // Disable player movement while minigame is active
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (pc != null) pc.canMove = false;

        // Generate random sequence
        KeyCode[] arrows = {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };
        sequence = new KeyCode[sequenceLength];
        for (int i = 0; i < sequenceLength; i++)
            sequence[i] = arrows[Random.Range(0, arrows.Length)];

        // Show the panel and update prompt
        minigamePanel.SetActive(true);
        UpdatePromptText();
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    void HandleInput(KeyCode key)
    {
        if (key == sequence[currentIndex])
        {
            // Correct key pressed
            currentIndex++;
            UpdatePromptText();

            if (currentIndex >= sequenceLength)
            {
                // All keys matched - success
                MinigameSuccess();
            }
        }
        else
        {
            // Wrong key pressed - failure
            MinigameFailure();
        }
    }

    void UpdatePromptText()
    {
        string display = "";
        for (int i = 0; i < sequence.Length; i++)
        {
            if (i < currentIndex)
                // Already entered correctly - show in gray
                display += "<color=#888888>" + GetArrowSymbol(sequence[i]) + "</color> ";
            else if (i == currentIndex)
                // Current key to press - show in white/bold
                display += "<b>" + GetArrowSymbol(sequence[i]) + "</b> ";
            else
                // Upcoming keys - show dimmed
                display += "<color=#444444>" + GetArrowSymbol(sequence[i]) + "</color> ";
        }
        promptText.text = display;
    }

    void MinigameSuccess()
    {
        Debug.Log("Minigame success!");
        currentContainer.CollectLoot();
        EndMinigame();
    }

    void MinigameFailure()
    {
        Debug.Log("Minigame failed! Guard alerted.");
        // Alert nearest guard - we'll expand this later
        EndMinigame();
    }

    void EndMinigame()
    {
        isActive = false;
        minigamePanel.SetActive(false);

        // Re-enable player movement
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (pc != null) pc.canMove = true;
    }
}