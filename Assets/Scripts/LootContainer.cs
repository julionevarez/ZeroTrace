using UnityEngine;

public class LootContainer : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private int lootValue = 1;
    [SerializeField] private Sprite lootedSprite;
    [SerializeField] private GameObject promptCanvas; // "Press E to Loot" prompt

    // ── PRIVATE STATE ────────────────────────────────────────────
    private bool isLooted = false;
    private bool isLocked = false;
    private bool playerInRange = false;
    private SpriteRenderer sr;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("SpriteRenderer not found on " + gameObject.name);

        // Make sure prompt is hidden on start
        if (promptCanvas != null)
            promptCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isLooted && !isLocked && Input.GetKeyDown(KeyCode.E))
        {
            // Hide prompt before starting minigame
            if (promptCanvas != null)
                promptCanvas.SetActive(false);
                
            LootMinigame.Instance.StartMinigame(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Show prompt only if crate is available
            if (!isLooted && !isLocked && promptCanvas != null)
                promptCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Hide prompt when player leaves
            if (promptCanvas != null)
                promptCanvas.SetActive(false);
        }
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    // Called when loot is successfully collected
    public void CollectLoot()
    {
        if (isLooted) return;

        isLooted = true;
        GameManager.Instance.lootCollected += lootValue;
        Debug.Log("Loot collected! Total: " + GameManager.Instance.lootCollected);

        // Hide prompt
        if (promptCanvas != null)
            promptCanvas.SetActive(false);

        // Swap to looted sprite
        if (sr != null && lootedSprite != null)
            sr.sprite = lootedSprite;
    }

    // Locks the crate permanently for this run
    public void LockCrate()
    {
        if (isLooted) return;

        isLocked = true;
        Debug.Log("Crate locked!");

        // Hide prompt
        if (promptCanvas != null)
            promptCanvas.SetActive(false);

        // Tint red to show it's locked
        if (sr != null)
            sr.color = new Color(0.5f, 0f, 0f, 1f);
    }
}