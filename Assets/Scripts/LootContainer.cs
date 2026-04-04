using UnityEngine;

public class LootContainer : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private int lootValue = 1;
    [SerializeField] private Sprite lootedSprite;
    [SerializeField] private Sprite lockedSprite;

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
    }

    void Update()
    {
        if (playerInRange && !isLooted && !isLocked && Input.GetKeyDown(KeyCode.E))
            LootMinigame.Instance.StartMinigame(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isLooted && !isLocked)
                Debug.Log("Press E to loot");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    public void CollectLoot()
    {
        if (isLooted) return;

        isLooted = true;
        GameManager.Instance.lootCollected += lootValue;
        Debug.Log("Loot collected! Total: " + GameManager.Instance.lootCollected);

        if (sr != null && lootedSprite != null)
            sr.sprite = lootedSprite;
    }

    // Locks the crate permanently for this run
    public void LockCrate()
    {
        if (isLooted) return;

        isLocked = true;
        Debug.Log("Crate locked!");

        // Tint red to show it's locked
        if (sr != null)
            sr.color = new Color(0.5f, 0f, 0f, 1f);
    }
}