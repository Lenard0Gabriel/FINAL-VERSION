using UnityEngine;

public class GarbageProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 5f;
    public int damageAmount = 1;
    public float lifetime = 5f;

    [Header("Sound Effects")]
    public AudioClip hitSound; // Assign in Inspector (optional)
    private AudioSource audioSource;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Move horizontally based on direction
        if (rb != null)
        {
            rb.velocity = new Vector2(speed, 0f);
        }

        // Destroy the object after its lifetime ends
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ✅ Check if player is hit
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                // Deal damage to player
                player.TakeDamage(damageAmount);

                // ✅ Show death reason via UIManager (if player died)
                if (player.currentHearts <= 0)
{
    UIManager ui = FindObjectOfType<UIManager>();
    if (ui != null)
    {
        ui.ShowDeathReason();
    }
}

            }

            // Play hit sound if available
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // Destroy garbage object (after short delay if sound exists)
            Destroy(gameObject, hitSound ? hitSound.length : 0f);
        }
        else if (other.CompareTag("Ground"))
        {
            // When garbage hits the ground, destroy it
            Destroy(gameObject, 0.2f);
        }
    }
}
