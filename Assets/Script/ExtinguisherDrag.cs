using UnityEngine;

public class ExtinguisherDrag : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 offset;
    private bool isDragging = false;
    private Camera mainCamera;

    private Animator animator;
    public GameObject smoke;

    void Start()
    {
        originalPosition = transform.position;
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();

        if (smoke != null)
            smoke.SetActive(false);
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            transform.position = worldPos + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        offset = transform.position - worldPos;

        if (animator != null) animator.SetBool("IsSmoking", true);
        if (smoke != null) smoke.SetActive(true);
    }

    void StopDragging()
    {
        isDragging = false;

        // Smooth return to original position instead of instant snap
        StartCoroutine(ReturnToOriginalPosition());

        if (animator != null) animator.SetBool("IsSmoking", false);
        if (smoke != null) smoke.SetActive(false);
    }

    private System.Collections.IEnumerator ReturnToOriginalPosition()
    {
        float duration = 0.25f; // return time in seconds
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // ensure exact position at end
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isDragging) return;

        // accept tag "Flame"
        if (!other.CompareTag("Flame")) return;

        // try UniversalFlame first
        var uf = other.GetComponent<UniversalFlame>();
        if (uf != null)
        {
            uf.ReduceTimer(Time.deltaTime);
            return;
        }

        // fallback to legacy FlameExtinguish
        var fe = other.GetComponent<FlameExtinguish>();
        if (fe != null)
        {
            fe.ReduceTimer(Time.deltaTime);
            return;
        }

        // fallback to Fire with immediate Extinguish (if you have such script)
        var fire = other.GetComponent<Fire>();
        if (fire != null)
        {
            // because older Fire had no ReduceTimer, call Extinguish immediately
            fire.Extinguish();
            return;
        }

        // nothing found on the flame collider
        Debug.LogWarning($"[ExtinguisherDrag] Collider {other.name} has tag Flame but no flame component found.");
    }
}
