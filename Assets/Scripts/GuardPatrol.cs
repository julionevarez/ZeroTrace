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

        // Start by moving toward WaypointA
        currentTarget = waypointA;
    }

    void Update()
    {
        if (currentTarget == null) return;

        // Move toward current waypoint
        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget.position,
            patrolSpeed * Time.deltaTime
        );

        // If close enough to current waypoint, switch to the other one
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        if (distanceToTarget < 0.1f)
        {
            currentTarget = (currentTarget == waypointA) ? waypointB : waypointA;
        }
    }
}