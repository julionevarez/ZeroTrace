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
    [SerializeField] private Canvas minigameCanvas;

    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private int sequenceLength = 4;
    [SerializeField] private float yOffset = 1.5f; // How far above the crate

    // ── PRIVATE STATE ────────────────────────────────────────────
    private KeyCode[] sequence;
    private int currentIndex = 0;
    private bool isActive = false;
    private LootContainer currentContainer;

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

    public void StartMinigame(LootContainer container)
    {
        currentContainer = container;
        currentIndex = 0;
        isActive = true;

        // Position canvas above the crate
        Vector3 cratePos = container.transform.position;
        minigameCanvas.transform.position = new Vector3(
            cratePos.x,
            cratePos.y + yOffset,
            cratePos.z
        );

        // Disable player movement
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

        minigamePanel.SetActive(true);
        UpdatePromptText();
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    void HandleInput(KeyCode key)
    {
        if (key == sequence[currentIndex])
        {
            currentIndex++;
            UpdatePromptText();

            if (currentIndex >= sequenceLength)
                MinigameSuccess();
        }
        else
        {
            MinigameFailure();
        }
    }

    void UpdatePromptText()
    {
        string display = "";
        for (int i = 0; i < sequence.Length; i++)
        {
            if (i < currentIndex)
                display += "<color=#888888>" + GetArrowSymbol(sequence[i]) + "</color> ";
            else if (i == currentIndex)
                display += "<b>" + GetArrowSymbol(sequence[i]) + "</b> ";
            else
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
        EndMinigame();
    }

    void EndMinigame()
    {
        isActive = false;
        minigamePanel.SetActive(false);

        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) pc.canMove = true;
    }
}