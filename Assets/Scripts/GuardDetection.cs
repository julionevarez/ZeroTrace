using UnityEngine;

public class GuardDetection : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    // How far the guard can see
    [SerializeField] private float detectionRadius = 3f;

    // How wide the guard's field of view is in degrees
    // 90 = quarter circle in front, 120 = wider cone, 60 = narrow
    [SerializeField] private float fieldOfView = 90f;

    // Which layers count as walls for the raycast
    [SerializeField] private LayerMask wallLayer;

    // ── PRIVATE REFS ─────────────────────────────────────────────
    // Reference to the guard's SpriteRenderer to determine facing direction
    private SpriteRenderer sr;

    // The four directional sprites — used to determine which way guard is facing
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        // Cache the SpriteRenderer on this guard
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Find the player by tag each frame
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // First check: is the player within detection radius?
        if (distanceToPlayer > detectionRadius) return;

        // Second check: is the player within the guard's field of view?
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 guardFacing = GetFacingDirection();

        // Calculate the angle between guard facing and direction to player
        // Dot product gives cosine of angle between two normalized vectors
        float dot = Vector2.Dot(guardFacing, directionToPlayer);
        float angleToPlayer = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // If player is outside the field of view cone, guard can't see them
        if (angleToPlayer > fieldOfView / 2f) return;

        // Third check: is there a wall between the guard and player?
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            distanceToPlayer,
            wallLayer
        );

        // If raycast hit nothing, guard has clear line of sight
        if (hit.collider == null)
        {
            Debug.Log("Guard spotted the player!");
            GameManager.Instance.PlayerSpotted();
        }
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    // Returns a normalized Vector2 representing which way the guard is facing
    // based on which directional sprite is currently displayed
    Vector2 GetFacingDirection()
    {
        if (sr == null) return Vector2.down;

        Sprite current = sr.sprite;

        if (current == spriteUp)    return Vector2.up;
        if (current == spriteDown)  return Vector2.down;
        if (current == spriteLeft)  return Vector2.left;
        if (current == spriteRight) return Vector2.right;

        // Default to down if no match found
        return Vector2.down;
    }
}