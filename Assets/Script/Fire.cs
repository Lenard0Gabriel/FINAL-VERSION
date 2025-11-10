using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public List<GameObject> spreadTargets;
    public float spreadTime = 3f;

    private bool isExtinguished = false;
    private float timer;
    private PuzzleManager puzzleManager;

    void Start()
    {
        timer = spreadTime;
        puzzleManager = PuzzleManager.Instance;

        if (puzzleManager != null)
        {
            puzzleManager.RegisterFire(gameObject); // Register at start
        }
        else
        {
            Debug.LogError("❌ PuzzleManager instance not found!");
        }
    }

    void Update()
    {
        if (!isExtinguished)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpreadFire();
                timer = spreadTime;
            }
        }
    }

    public void Extinguish()
    {
        if (isExtinguished) return;

        isExtinguished = true;

        if (puzzleManager != null)
        {
            puzzleManager.UnregisterFire(gameObject); // Remove from active list
        }

        enabled = false; // Stop Update
        Destroy(gameObject);
    }

    void SpreadFire()
    {
        foreach (GameObject target in spreadTargets)
        {
            if (target == null) continue;

            if (!target.activeSelf)
            {
                target.SetActive(true);

                // ✅ Register new flame
                puzzleManager?.RegisterFire(target);

                // ✅ Check for game over
                puzzleManager?.CheckForGameOver();
                break; // Only spread to one target per timer
            }
        }
    }
}
