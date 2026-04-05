using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private float moveSpeed = 5f;

    // ── PRIVATE REFS ─────────────────────────────────────────────
    private Rigidbody2D rb;
    private Vector2 moveInput;

    // ── PUBLIC STATE ─────────────────────────────────────────────
    // Used by LootMinigame to disable movement during input sequence
    public bool canMove = true;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        // Grab the Rigidbody2D attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    if (!canMove)
    {
        moveInput = Vector2.zero;
        AudioManager.Instance.SetFootsteps(false);
        return;
    }

    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");
    moveInput = new Vector2(moveX, moveY).normalized;

    // Play footsteps when moving
    AudioManager.Instance.SetFootsteps(moveInput.magnitude > 0);
    }

    void FixedUpdate()
    {
        // Apply movement via physics — FixedUpdate keeps it frame-rate independent
        rb.linearVelocity = moveInput * moveSpeed;
    }
}