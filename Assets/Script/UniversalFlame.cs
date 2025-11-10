using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalFlame : MonoBehaviour
{
    [Header("Extinguish Settings")]
    public float extinguishTime = 2.5f;

    [Header("Spread Settings")]
    public List<GameObject> spreadTargets;
    public float spreadInterval = 3f;

    private float spreadTimer;
    private bool isExtinguished = false;
    private PuzzleManager puzzleManager;

    void OnEnable()
    {
        // make sure tag exists and is set so extinguisher's tag check works
        if (!CompareTag("Flame"))
            gameObject.tag = "Flame";
    }

    void Start()
    {
        spreadTimer = spreadInterval;
        StartCoroutine(EnsureRegistered());
    }

    private IEnumerator EnsureRegistered()
    {
        // wait up to a short timeout for PuzzleManager to exist
        float wait = 0f;
        while (PuzzleManager.Instance == null && wait < 2f)
        {
            wait += Time.deltaTime;
            yield return null;
        }

        puzzleManager = PuzzleManager.Instance;
        if (puzzleManager != null)
        {
            puzzleManager.RegisterFire(gameObject);
        }
        else
        {
            Debug.LogError($"[UniversalFlame] PuzzleManager not found when registering {name}!");
        }
    }

    void Update()
    {
        if (isExtinguished) return;

        spreadTimer -= Time.deltaTime;
        if (spreadTimer <= 0f)
        {
            SpreadFire();
            spreadTimer = spreadInterval;
        }
    }

    public void ReduceTimer(float amount)
    {
        if (isExtinguished) return;

        extinguishTime -= amount;
        // debug:
        // Debug.Log($"{name} timer: {extinguishTime:F2}");

        if (extinguishTime <= 0f)
        {
            Extinguish();
        }
    }

    public void Extinguish()
    {
        if (isExtinguished) return;

        isExtinguished = true;
        Debug.Log($"[UniversalFlame] Extinguished: {name}");

        puzzleManager?.UnregisterFire(gameObject);

        // optional short delay for VFX, but destroy now
        Destroy(gameObject);
    }

    private void SpreadFire()
    {
        foreach (GameObject target in spreadTargets)
        {
            if (target == null) continue;
            if (!target.activeSelf)
            {
                target.SetActive(true);
                // register new active target
                PuzzleManager.Instance?.RegisterFire(target);
                Debug.Log($"[UniversalFlame] Spread: activated {target.name}");
                break;
            }
        }
    }
}
