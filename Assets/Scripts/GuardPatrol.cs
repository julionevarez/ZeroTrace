using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    // Array of waypoints the guard will patrol between in order
    // Assign in Inspector by setting array size and dragging waypoint GameObjects
    [SerializeField] private Transform[] waypoints;

    // How fast the guard moves between waypoints
    // Can be tuned per guard in the Inspector to vary patrol speeds
    [SerializeField] private float patrolSpeed = 2f;

    // When true, guard reverses direction at the last waypoint
    // instead of looping back to the first waypoint
    // Example: A -> B -> C -> B -> A -> B -> C (ping pong)
    // vs       A -> B -> C -> A -> B -> C (loop)
    [SerializeField] private bool pingPong = false;

    // ── DIRECTIONAL SPRITES ──────────────────────────────────────
    // Four sprites for each facing direction
    // Swapped automatically based on which direction the guard is moving
    [SerializeField] private Sprite spriteUp;    // Back of guard
    [SerializeField] private Sprite spriteDown;  // Front of guard
    [SerializeField] private Sprite spriteLeft;  // Left side profile
    [SerializeField] private Sprite spriteRight; // Right side profile

    // ── PRIVATE STATE ────────────────────────────────────────────
    // Index of the waypoint the guard is currently moving toward
    private int currentWaypointIndex = 0;

    // Used for ping pong mode: 1 = moving forward, -1 = moving backward
    private int direction = 1;

    // Cached component references
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        // Cache components on startup to avoid expensive GetComponent calls each frame
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Don't do anything if no waypoints are assigned
        if (waypoints.Length == 0) return;

        // Get the current target waypoint
        Transform target = waypoints[currentWaypointIndex];

        // Calculate the direction vector from guard to target
        // Used to determine which directional sprite to display
        Vector2 moveDir = ((Vector2)target.position - rb.position).normalized;

        // Swap sprite to face the direction of movement
        UpdateSprite(moveDir);

        // Move toward the current waypoint using MoveTowards
        // This respects physics and wall colliders unlike transform.position
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            target.position,
            patrolSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPosition);

        // Check if guard has reached the current waypoint
        if (Vector2.Distance(rb.position, target.position) < 0.1f)
        {
            if (pingPong)
            {
                // Reverse direction when reaching either end of the waypoint list
                if (currentWaypointIndex >= waypoints.Length - 1)
                    direction = -1; // Reached last waypoint, start going backward
                else if (currentWaypointIndex <= 0)
                    direction = 1;  // Reached first waypoint, start going forward

                // Move to next waypoint in current direction
                currentWaypointIndex += direction;
            }
            else
            {
                // Loop mode: wrap back to first waypoint after reaching the last
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    void UpdateSprite(Vector2 moveDir)
    {
        // Prioritize horizontal direction when moving diagonally
        // This ensures left/right sprites show for mostly-horizontal movement
        if (Mathf.Abs(moveDir.x) >= Mathf.Abs(moveDir.y))
        {
            // Guard is moving more horizontally
            if (moveDir.x > 0 && spriteRight != null)
                sr.sprite = spriteRight; // Moving right
            else if (moveDir.x < 0 && spriteLeft != null)
                sr.sprite = spriteLeft;  // Moving left
        }
        else
        {
            // Guard is moving more vertically
            if (moveDir.y > 0 && spriteUp != null)
                sr.sprite = spriteUp;    // Moving up
            else if (moveDir.y < 0 && spriteDown != null)
                sr.sprite = spriteDown;  // Moving down
        }
    }
}