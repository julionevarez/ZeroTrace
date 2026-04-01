using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private Transform waypointA;
    [SerializeField] private Transform waypointB;
    [SerializeField] private float patrolSpeed = 2f;

    // ── PRIVATE STATE ────────────────────────────────────────────
    private Transform currentTarget;
    private Rigidbody2D rb;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = waypointA;
    }

    void FixedUpdate()
    {
        if (currentTarget == null) return;

        // Move via physics so the guard respects wall colliders
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            currentTarget.position,
            patrolSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPosition);

        // Switch waypoint when close enough
        float distanceToTarget = Vector2.Distance(rb.position, currentTarget.position);
        if (distanceToTarget < 0.1f)
        {
            // Toggle between waypointA and waypointB
            currentTarget = (currentTarget == waypointA) ? waypointB : waypointA;
        }
    }
}