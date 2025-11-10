using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 5;
    public int currentHearts;

    public Transform respawnPoint;
    public GameObject[] heartIcons; // Optional: assign your UI heart images

    public bool isHidden = false; // ✅ Tracks if player is hiding

    private UIManager uiManager; // ✅ Cache the UI reference

    private void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
        uiManager = FindObjectOfType<UIManager>();
        Debug.Log("PlayerHealth initialized with " + currentHearts + " hearts.");
    }

    private void Update()
    {
        // Debug test: Press T key to simulate damage
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test damage input triggered.");
            TakeDamage(1, "Test damage (T key pressed)");
        }
    }

    // ✅ Overloaded TakeDamage to include reason
    public void TakeDamage(int amount, string reason = "")
    {
        if (currentHearts <= 0)
        {
            Debug.Log("TakeDamage called but player is already at 0 HP.");
            return;
        }

        Debug.Log("TakeDamage called! Damage: " + amount + " | Reason: " + reason);

        int oldHearts = currentHearts;
        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        UpdateHeartsUI();

        // ✅ Show tip only when player takes damage but is still alive
        if (currentHearts > 0 && currentHearts < oldHearts)
        {
            if (uiManager != null)
            {
                uiManager.ShowDeathReason();
            }
            else
            {
                Debug.LogWarning("UIManager not found in scene when showing tip!");
            }

            Debug.Log("Player still alive with " + currentHearts + " hearts. Respawning...");
            Respawn();
        }
        else if (currentHearts <= 0)
        {
            Debug.Log("Player died.");
            Die();
        }
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            Debug.Log("Player respawned at: " + respawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Respawn point not assigned!");
        }

        // Reset physics state
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Die()
    {
        // ✅ No tips when fully dead — only handle Game Over
        Debug.Log("Executing Die() — player fully dead.");

        // Show Game Over screen if GameOverManager is set
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("GameOverManager.Instance is null!");
        }

        // Optional: Disable player GameObject on death
        gameObject.SetActive(false);
    }

    void UpdateHeartsUI()
    {
        if (heartIcons == null || heartIcons.Length == 0) return;

        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentHearts);
        }

        Debug.Log("Hearts UI updated. Current HP: " + currentHearts);
    }
}
