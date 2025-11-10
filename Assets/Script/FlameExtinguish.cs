using UnityEngine;

public class FlameExtinguish : MonoBehaviour
{
    public float extinguishTime = 2.5f;
    private float timer;

    private bool isExtinguished = false;

    void Start()
    {
        timer = extinguishTime;

        // Register this flame with PuzzleManager
        if (PuzzleManager.Instance != null)
        {
            PuzzleManager.Instance.RegisterFire(gameObject);
        }
        else
        {
            Debug.LogError("PuzzleManager instance not found!");
        }
    }

    // Called by extinguisher while dragging
    public void ReduceTimer(float amount)
    {
        if (isExtinguished) return;

        timer -= amount;

        if (timer <= 0f)
        {
            ExtinguishFire();
        }
    }

    // ✅ Extinguish the flame
    void ExtinguishFire()
    {
        if (isExtinguished) return;

        isExtinguished = true;

        if (PuzzleManager.Instance != null)
        {
            PuzzleManager.Instance.UnregisterFire(gameObject);
        }

        Destroy(gameObject);
    }
}
