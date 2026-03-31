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
        // Don't read input if movement is disabled (e.g. during minigame)
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        // Read raw WASD / arrow key input (-1, 0, or 1 on each axis)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Normalize so diagonal movement isn't faster than cardinal
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Apply movement via physics — FixedUpdate keeps it frame-rate independent
        rb.linearVelocity = moveInput * moveSpeed;
    }
}