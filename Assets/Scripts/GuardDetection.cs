using UnityEngine;

public class GuardDetection : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask wallLayer;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Update()
    {
        // Find the player by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Check if player is within detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            // Cast a ray toward the player
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                directionToPlayer,
                distanceToPlayer,
                wallLayer
            );

            // If ray didn't hit a wall, guard can see the player
            if (hit.collider == null)
            {
                Debug.Log("Guard spotted the player!");
                GameManager.Instance.PlayerSpotted();
            }
        }
    }
}