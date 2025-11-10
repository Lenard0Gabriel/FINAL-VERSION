using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    public bool isHidden = false;
    private Collider2D nearbyShelter;
    private SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth; // ✅ Reference to PlayerHealth

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>(); // ✅ Get PlayerHealth on start
    }

    void Update()
    {
        // Detect nearby shelter
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        nearbyShelter = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Shelter"))
            {
                nearbyShelter = hit;
                break;
            }
        }
    }

    public void ToggleHide()
    {
        if (isHidden)
        {
            Unhide();
        }
        else if (nearbyShelter != null)
        {
            Hide();
        }
        else
        {
            Debug.Log("No shelter nearby to hide.");
        }
    }

    public void Hide()
    {
        isHidden = true;
        spriteRenderer.enabled = false;
        if (playerHealth != null) playerHealth.isHidden = true; // ✅ Sync with PlayerHealth
        Debug.Log("You are now hiding.");
    }

    public void Unhide()
    {
        isHidden = false;
        spriteRenderer.enabled = true;
        if (playerHealth != null) playerHealth.isHidden = false; // ✅ Sync with PlayerHealth
        Debug.Log("You are no longer hiding.");
    }
}
