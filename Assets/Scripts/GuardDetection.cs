using UnityEngine;

public class GuardDetection : MonoBehaviour
{
    // ── UNITY METHODS ────────────────────────────────────────────

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Guard spotted the player!");
            GameManager.Instance.PlayerSpotted();
        }
    }
}