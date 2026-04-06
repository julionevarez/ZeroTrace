using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    // moveSpeed controls how fast the player moves across the map
    // Exposed to Inspector so it can be tuned without touching code
    [SerializeField] private float moveSpeed = 5f;

    // ── DIRECTIONAL SPRITES ──────────────────────────────────────
    // Four sprites representing the player facing each direction
    // Assigned in the Inspector by dragging the appropriate PNG
    [SerializeField] private Sprite spriteUp;    // Back of player visible
    [SerializeField] private Sprite spriteDown;  // Front of player visible
    [SerializeField] private Sprite spriteLeft;  // Left side profile
    [SerializeField] private Sprite spriteRight; // Right side profile

    // ── PRIVATE REFS ─────────────────────────────────────────────
    // Rigidbody2D handles physics-based movement
    // SpriteRenderer handles which sprite is currently displayed
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Stores the current movement direction as a normalized Vector2
    // Updated every frame in Update() and applied in FixedUpdate()
    private Vector2 moveInput;

    // ── PUBLIC STATE ─────────────────────────────────────────────
    // Set to false by LootMinigame to freeze the player during minigame
    // Set back to true by LootMinigame when minigame ends
    public bool canMove = true;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        // Cache component references on startup
        // Avoids calling GetComponent every frame which is expensive
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // If movement is disabled (e.g. during minigame), stop everything
        if (!canMove)
        {
            moveInput = Vector2.zero;
            AudioManager.Instance.SetFootsteps(false);
            return;
        }

        // GetAxisRaw returns -1, 0, or 1 with no smoothing
        // This gives tight, responsive movement without input lag
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Normalize prevents diagonal movement from being faster
        // than cardinal movement (without normalize diagonal = 1.41x speed)
        moveInput = new Vector2(moveX, moveY).normalized;

        // Swap sprite to match the direction the player is moving
        UpdateSprite(moveX, moveY);

        // Play footstep audio while moving, stop when standing still
        // magnitude > 0 means the player is pressing at least one direction key
        AudioManager.Instance.SetFootsteps(moveInput.magnitude > 0);
    }

    void FixedUpdate()
    {
        // Apply movement via physics in FixedUpdate for frame-rate independence
        // Using linearVelocity instead of AddForce for direct, predictable control
        rb.linearVelocity = moveInput * moveSpeed;
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    void UpdateSprite(float moveX, float moveY)
    {
        // Don't update sprite if player is standing still
        // This keeps the last facing direction when idle
        if (moveX == 0 && moveY == 0) return;

        // When moving diagonally, prioritize horizontal direction
        // This gives left/right sprites priority over up/down
        // which feels more natural for top-down movement
        if (Mathf.Abs(moveX) >= Mathf.Abs(moveY))
        {
            // Moving more horizontally than vertically
            if (moveX > 0 && spriteRight != null)
                sr.sprite = spriteRight;
            else if (moveX < 0 && spriteLeft != null)
                sr.sprite = spriteLeft;
        }
        else
        {
            // Moving more vertically than horizontally
            if (moveY > 0 && spriteUp != null)
                sr.sprite = spriteUp;
            else if (moveY < 0 && spriteDown != null)
                sr.sprite = spriteDown;
        }
    }
}