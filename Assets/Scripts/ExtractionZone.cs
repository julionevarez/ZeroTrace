using UnityEngine;

public class ExtractionZone : MonoBehaviour
{
    // ── PRIVATE STATE ────────────────────────────────────────────
    private SpriteRenderer sr;

    // ── SETTINGS ────────────────────────────────────────────────
    // Pulse speed for the glow effect
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 0.5f;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Pulse the alpha to create a glowing effect
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, 
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        sr.color = new Color(0f, 1f, 0f, alpha);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.lootCollected > 0)
            {
                // Player has loot — trigger win
                Debug.Log("Extracted successfully!");
                GameManager.Instance.WinGame();
            }
            else
            {
                // Player has no loot — don't extract
                Debug.Log("You need loot to extract!");
            }
        }
    }
}