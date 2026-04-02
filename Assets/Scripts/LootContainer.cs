using UnityEngine;

public class LootContainer : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private int lootValue = 1;
    [SerializeField] private Sprite lootedSprite;

    // ── PRIVATE STATE ────────────────────────────────────────────
    private bool isLooted = false;
    private bool playerInRange = false;
    private SpriteRenderer sr;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("SpriteRenderer not found on " + gameObject.name);
    }

    void Update()
    {
        // If player is in range and presses E, trigger the minigame
        if (playerInRange && !isLooted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Starting loot minigame!");
            // Minigame will be triggered here in Step 7
            // For now collect directly to test
            CollectLoot();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press E to loot");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    // Called when loot is successfully collected (will be called by minigame later)
    public void CollectLoot()
    {
        if (isLooted) return;

        isLooted = true;
        GameManager.Instance.lootCollected += lootValue;
        Debug.Log("Loot collected! Total: " + GameManager.Instance.lootCollected);

        // Only swap sprite if both references are valid
        if (sr != null && lootedSprite != null)
            sr.sprite = lootedSprite;
    }
}