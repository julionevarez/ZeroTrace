using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;

    // ── PRIVATE STATE ────────────────────────────────────────────
    private int currentWaypointIndex = 0;
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
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}