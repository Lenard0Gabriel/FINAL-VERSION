using UnityEngine;

public class WaterHazard : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if player isSafe
            MobilePlayerMovement movement = other.GetComponent<MobilePlayerMovement>();
            if (movement != null && movement.isSafe)
                return; // ✅ Player is safe, don't apply damage

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
