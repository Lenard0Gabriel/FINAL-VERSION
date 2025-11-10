using UnityEngine;
using UnityEngine.SceneManagement;

public class FloodPuzzleManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject floodPuzzlePanel;
    public GameObject startButton1;
    public GameObject startButton2;
    public GameObject startButton3;
    public GameObject missionCompleteUI;
    public GameObject missionFailedUI;

    [Header("References")]
    public GameObject player;

    [Header("Timer Settings")]
    public float timeLimit = 30f;
    private float remainingTime;
    private bool puzzleActive = false;

    [Header("Scene Settings")]
    public string puzzleSceneName = "FloodPuzzleScene";
    public string puzzleScene2Name = "QAScene";
    public string puzzleScene3Name = "BagPuzzleScene";
    public string returnFloodSceneName = "ReturnFlood";
    public string floodSceneName = "Flood";   // ✅ target scene (short name)

    private bool panelShown = false;

    private void Start()
    {
        if (floodPuzzlePanel != null) floodPuzzlePanel.SetActive(false);
        if (startButton1 != null) startButton1.SetActive(false);
        if (startButton2 != null) startButton2.SetActive(false);
        if (startButton3 != null) startButton3.SetActive(false);
        if (missionCompleteUI != null) missionCompleteUI.SetActive(false);
        if (missionFailedUI != null) missionFailedUI.SetActive(false);
    }

    private void Update()
    {
        if (puzzleActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                MissionFailed();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (panelShown) return;

        bool playerTouchedGuide = false;

        if (this.CompareTag("Guide") && other.CompareTag("Player"))
            playerTouchedGuide = true;

        if (this.CompareTag("Player") && other.CompareTag("Guide"))
            playerTouchedGuide = true;

        if (playerTouchedGuide)
        {
            ShowPanel();
            panelShown = true;
        }
    }

    private void ShowPanel()
    {
        if (floodPuzzlePanel != null) floodPuzzlePanel.SetActive(true);

        if (startButton1 != null) startButton1.SetActive(true);
        if (startButton2 != null) startButton2.SetActive(true);
        if (startButton3 != null) startButton3.SetActive(true);
    }

    // Puzzle scenes
    public void LoadFloodPuzzleScene()
    {
        if (floodPuzzlePanel != null) floodPuzzlePanel.SetActive(false);
        SceneManager.LoadScene(puzzleSceneName);
    }

    public void LoadQAScene()
    {
        if (floodPuzzlePanel != null) floodPuzzlePanel.SetActive(false);
        SceneManager.LoadScene(puzzleScene2Name);
    }

    public void LoadBagPuzzleScene()
    {
        if (floodPuzzlePanel != null) floodPuzzlePanel.SetActive(false);
        SceneManager.LoadScene(puzzleScene3Name);
    }

    // Mission UI
    public void MissionComplete()
    {
        puzzleActive = false;
        if (missionCompleteUI != null) missionCompleteUI.SetActive(true);
    }

    public void MissionFailed()
    {
        puzzleActive = false;
        if (missionFailedUI != null) missionFailedUI.SetActive(true);
    }

    // ✅ ReturnFlood
    public void LoadReturnFloodScene()
    {
        SceneManager.LoadScene(returnFloodSceneName);
    }

    // ✅ Flood scene loader with name check + fallback
    public void LoadFloodScene()
    {
        Debug.Log("Attempting to load Flood scene...");

        // Try direct name
        try
        {
            SceneManager.LoadScene(floodSceneName);
            return;
        }
        catch
        {
            Debug.LogWarning("Scene with name '" + floodSceneName + "' not found. Trying with path prefix...");
        }

        // Try with folder prefix
        try
        {
            SceneManager.LoadScene("Scenes/Main Scene/Flood");
            return;
        }
        catch
        {
            Debug.LogWarning("Scene 'Scenes/Main Scene/Flood' not found. Falling back to Build Index 1.");
        }

        // Final fallback: Build Index
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("No valid Flood scene found in Build Settings!");
        }
    }
}
