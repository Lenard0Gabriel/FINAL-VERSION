using UnityEngine;
using UnityEngine.SceneManagement;

public class FirePuzzleManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject firePuzzlePanel;    // Main puzzle intro panel
    public GameObject startButton1;       // Start button for FirePuzzleScene
    public GameObject startButton2;       // Start button for FirePuzzle2
    public GameObject startButton3;       // Start button for FirePuzzle3
    public GameObject missionCompleteUI;  // Mission complete panel
    public GameObject missionFailedUI;    // Mission failed panel

    [Header("References")]
    public GameObject player;             // Assign Player in inspector

    [Header("Timer Settings")]
    public float timeLimit = 30f; // Time to complete the mission
    private float remainingTime;
    private bool puzzleActive = false;

    [Header("Scene Settings")]
    public string puzzleSceneName = "FirePuzzleScene";   // Change in Inspector if needed
    public string puzzleScene2Name = "FirePuzzle2";      // Scene for FirePuzzle2
    public string puzzleScene3Name = "FirePuzzle3";      // Scene for FirePuzzle3
    public string fireQASceneName = "FireQAScene";       // Scene for FireQA
    public string returnFireSceneName = "ReturnFire";    // Scene to return to

    private void Start()
    {
        // Hide all panels and buttons at start
        if (firePuzzlePanel != null) firePuzzlePanel.SetActive(false);
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
        // Player collides with extinguisher
        if (other.CompareTag("Extinguisher") && gameObject.CompareTag("Player"))
        {
            ShowPanel();
        }
    }

    private void ShowPanel()
    {
        if (firePuzzlePanel != null) firePuzzlePanel.SetActive(true);

        // Show all buttons immediately (no delay)
        if (startButton1 != null) startButton1.SetActive(true);
        if (startButton2 != null) startButton2.SetActive(true);
        if (startButton3 != null) startButton3.SetActive(true);
    }

    // Scene loading functions
    public void LoadFirePuzzleScene()
    {
        if (firePuzzlePanel != null) firePuzzlePanel.SetActive(false);
        SceneManager.LoadScene(puzzleSceneName);
    }

    public void LoadFirePuzzle2Scene()
    {
        if (firePuzzlePanel != null) firePuzzlePanel.SetActive(false);
        SceneManager.LoadScene(puzzleScene2Name);
    }

    public void LoadFirePuzzle3Scene()
    {
        if (firePuzzlePanel != null) firePuzzlePanel.SetActive(false);
        SceneManager.LoadScene(puzzleScene3Name);
    }

    public void LoadFireQAScene()
    {
        if (firePuzzlePanel != null) firePuzzlePanel.SetActive(false);
        SceneManager.LoadScene(fireQASceneName);
    }

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

    public void LoadBackToFireScene()
    {
        SceneManager.LoadScene("Fire");
    }

    public void LoadReturnFireScene()
    {
        SceneManager.LoadScene(returnFireSceneName);
    }
}
