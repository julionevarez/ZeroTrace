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
    [SerializeField] private Image minigameTimerFill;

    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private int sequenceLength = 4;
    [SerializeField] private float yOffset = 1.5f;
    [SerializeField] private float minigameTimeLimit = 5f;

    // ── MINIGAME TIMER COLORS ────────────────────────────────────
    private Color timerGreen  = new Color(0f, 1f, 0f, 1f);
    private Color timerOrange = new Color(1f, 0.5f, 0f, 1f);
    private Color timerRed    = new Color(1f, 0f, 0f, 1f);

    // ── PRIVATE STATE ────────────────────────────────────────────
    private KeyCode[] sequence;
    private int currentIndex = 0;
    private bool isActive = false;
    private float minigameTimeRemaining;
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

        // Count down minigame timer
        minigameTimeRemaining -= Time.deltaTime;
        minigameTimeRemaining = Mathf.Max(minigameTimeRemaining, 0f);

        // Update minigame timer bar
        float fillAmount = minigameTimeRemaining / minigameTimeLimit;
        minigameTimerFill.fillAmount = fillAmount;

        // MGS-style color on minigame timer
        if (fillAmount > 0.5f)
            minigameTimerFill.color = timerGreen;
        else if (fillAmount > 0.25f)
            minigameTimerFill.color = timerOrange;
        else
            minigameTimerFill.color = timerRed;

        // Timer ran out — lock the crate
        if (minigameTimeRemaining <= 0f)
        {
            Debug.Log("Minigame timed out! Crate locked.");
            currentContainer.LockCrate();
            EndMinigame();
            return;
        }

        // Check arrow key input
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
        minigameTimeRemaining = minigameTimeLimit;

        // Position canvas above the crate
        Vector3 cratePos = container.transform.position;
        minigameCanvas.transform.position = new Vector3(
            cratePos.x,
            cratePos.y + yOffset,
            cratePos.z
        );

        // Disable player movement
        PlayerController pc = FindFirstObjectByType<PlayerController>();
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
        minigameTimerFill.fillAmount = 1f;
        minigameTimerFill.color = timerGreen;
        UpdatePromptText();

        AudioManager.Instance.PlayBoxOpen();
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

        AudioManager.Instance.PlayLootCollected();
    }

    void MinigameFailure()
    {
        Debug.Log("Minigame failed! Crate locked.");
        currentContainer.LockCrate();
        EndMinigame();
        AudioManager.Instance.PlayLootFail();
    }

    void EndMinigame()
    {
        isActive = false;
        minigamePanel.SetActive(false);

        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) pc.canMove = true;
    }
}