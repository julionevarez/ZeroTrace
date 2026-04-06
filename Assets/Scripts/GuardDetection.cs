using UnityEngine;

public class GuardDetection : MonoBehaviour
{
    // ── SETTINGS ────────────────────────────────────────────────
    // How far the guard can see ahead
    [SerializeField] private float detectionRadius = 3f;

    // Width of the guard's field of view in degrees
    // 90 = narrow forward cone, 120 = wider view
    [SerializeField] private float fieldOfView = 90f;

    // Wall layer for raycasting — only walls block line of sight
    [SerializeField] private LayerMask wallLayer;

    // ── SPRITE REFS ──────────────────────────────────────────────
    // Used to determine which direction the guard is currently facing
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    // ── PRIVATE REFS ─────────────────────────────────────────────
    // Guard's SpriteRenderer — checked every frame to get current facing direction
    private SpriteRenderer sr;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Start()
    {
        // Cache SpriteRenderer on startup
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Find player every frame by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // First check: is player within detection radius?
        if (distanceToPlayer > detectionRadius) return;

        // Second check: is player within the guard's field of view cone?
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 guardFacing = GetFacingDirection();

        // Dot product gives cosine of angle between guard facing and player direction
        float dot = Vector2.Dot(guardFacing, directionToPlayer);
        float angleToPlayer = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // Player outside FOV cone — guard can't see them
        if (angleToPlayer > fieldOfView / 2f) return;

        // Third check: is there a wall blocking line of sight?
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            distanceToPlayer,
            wallLayer
        );

        // No wall hit — guard has clear line of sight to player
        if (hit.collider == null)
        {
            Debug.Log("Guard spotted the player!");
            GameManager.Instance.PlayerSpotted();
        }
    }

    // Called when player walks directly into the guard's body trigger
    // Catches contact even when player approaches from behind
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Guard spotted the player — direct contact!");
            GameManager.Instance.PlayerSpotted();
        }
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    // Returns normalized Vector2 for the direction the guard is currently facing
    // based on which directional sprite is displayed
    Vector2 GetFacingDirection()
    {
        if (sr == null) return Vector2.down;

        Sprite current = sr.sprite;

        if (current == spriteUp)    return Vector2.up;
        if (current == spriteDown)  return Vector2.down;
        if (current == spriteLeft)  return Vector2.left;
        if (current == spriteRight) return Vector2.right;

        // Default facing direction if no sprite match found
        return Vector2.down;
    }
}