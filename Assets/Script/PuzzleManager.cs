using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("UI Panels")]
    public GameObject missionCompletePanel;
    public GameObject gameOverPanel;

    [Header("Game Settings")]
    public int maxAllowedFlames = 4;

    private List<GameObject> flames = new List<GameObject>();
    private bool puzzleEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[PuzzleManager] Instance set.");
        }
        else
        {
            Debug.LogWarning("[PuzzleManager] Duplicate instance destroyed.");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        puzzleEnded = false;

        if (missionCompletePanel != null) missionCompletePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        flames.Clear();

        // Register universal flame types found in scene
        UniversalFlame[] ufs = FindObjectsOfType<UniversalFlame>();
        foreach (var uf in ufs) RegisterFire(uf.gameObject);

        FlameExtinguish[] fes = FindObjectsOfType<FlameExtinguish>();
        foreach (var fe in fes) RegisterFire(fe.gameObject);

        Fire[] fires = FindObjectsOfType<Fire>();
        foreach (var f in fires) RegisterFire(f.gameObject);

        Debug.Log("[PuzzleManager] Initial active flames: " + flames.Count);
        CheckForGameOver();
    }

    public void RegisterFire(GameObject flame)
    {
        if (flame == null) return;
        if (!flames.Contains(flame))
        {
            flames.Add(flame);
            Debug.Log($"[PuzzleManager] Registered flame: {flame.name}. Count = {flames.Count}");
            CheckForGameOver();
        }
    }

    public void UnregisterFire(GameObject flame)
    {
        if (flame == null) return;
        if (flames.Contains(flame))
        {
            flames.Remove(flame);
            Debug.Log($"[PuzzleManager] Unregistered flame: {flame.name}. Remaining = {flames.Count}");

            // check for mission complete after a short delay to avoid race conditions with destroyed objects
            if (flames.Count <= 0 && !puzzleEnded)
            {
                StartCoroutine(DelayedMissionComplete());
            }
        }
    }

    private IEnumerator DelayedMissionComplete()
    {
        yield return null; // wait 1 frame to ensure all destruction events finish
        if (flames.Count <= 0 && !puzzleEnded)
        {
            ShowLevelComplete();
        }
    }

    public void CheckForGameOver()
    {
        if (!puzzleEnded && flames.Count >= maxAllowedFlames)
        {
            TriggerGameOver();
        }
    }

    private void ShowLevelComplete()
    {
        puzzleEnded = true;
        Debug.Log("[PuzzleManager] ✅ Mission Complete Triggered!");

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(true);
        else
            Debug.LogError("[PuzzleManager] missionCompletePanel not assigned!");
    }

    public void TriggerGameOver()
    {
        if (!puzzleEnded)
        {
            puzzleEnded = true;
            Debug.Log("[PuzzleManager] ❌ Game Over Triggered!");

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
            else
                Debug.LogError("[PuzzleManager] gameOverPanel not assigned!");
        }
    }
}
