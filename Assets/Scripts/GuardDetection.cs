using UnityEngine;

public class GuardDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Guard spotted the player!");
            GameManager.Instance.PlayerSpotted();
        }
    }
}