using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private bool pingPong = false; // Enable for back-and-forth

    // ── PRIVATE STATE ────────────────────────────────────────────
    private int currentWaypointIndex = 0;
    private int direction = 1; // 1 = forward, -1 = backward
    private Rigidbody2D rb;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];

        // Move toward current waypoint
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            target.position,
            patrolSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPosition);

        // Switch to next waypoint when close enough
        if (Vector2.Distance(rb.position, target.position) < 0.1f)
        {
            if (pingPong)
            {
                // Reverse direction at endpoints
                if (currentWaypointIndex >= waypoints.Length - 1)
                    direction = -1;
                else if (currentWaypointIndex <= 0)
                    direction = 1;

                currentWaypointIndex += direction;
            }
            else
            {
                // Loop forward
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }
}