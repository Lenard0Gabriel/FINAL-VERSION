using UnityEngine;
using UnityEngine.UI;

public class MegaScenarioManager : MonoBehaviour
{
    [Header("Main UI References")]
    public GameObject mainPanel;       // "SPP"
    public GameObject puzzleButtons;   // Puzzle Buttons group (1–3)

    [Header("Puzzle Panels")]
    public GameObject stormPipePanel;      // Puzzle 1
    public GameObject hazardMiniGamePanel; // Puzzle 2
    public GameObject miniGamePanel;       // Puzzle 3

    [Header("Puzzle Buttons")]
    public Button puzzleButton1;
    public Button puzzleButton2;
    public Button puzzleButton3;

    private bool triggeredOnce = false;   // Prevents re-trigger in same run
    private bool puzzleDone = false;      // Ensures only 1 completion

    private void Start()
    {
        // Hide everything at the start
        if (mainPanel != null) mainPanel.SetActive(false);
        if (puzzleButtons != null) puzzleButtons.SetActive(false);
        if (stormPipePanel != null) stormPipePanel.SetActive(false);
        if (hazardMiniGamePanel != null) hazardMiniGamePanel.SetActive(false);
        if (miniGamePanel != null) miniGamePanel.SetActive(false);

        // Hook up button listeners
        if (puzzleButton1 != null) puzzleButton1.onClick.AddListener(OpenStormPipePuzzle);
        if (puzzleButton2 != null) puzzleButton2.onClick.AddListener(OpenHazardMiniGame);
        if (puzzleButton3 != null) puzzleButton3.onClick.AddListener(OpenMiniGamePuzzle);

        // ✅ Check if a puzzle was saved before exiting
        int savedPuzzle = PlayerPrefs.GetInt("StormPuzzleIndex", 0);
        if (savedPuzzle > 0)
        {
            OpenPuzzleByIndex(savedPuzzle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggeredOnce && other.CompareTag("Player") && !puzzleDone)
        {
            ShowMainPanel();
            triggeredOnce = true;
        }
    }

    private void ShowMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (puzzleButtons != null) puzzleButtons.SetActive(true);
    }

    private void CloseMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (puzzleButtons != null) puzzleButtons.SetActive(false);
    }

    private void OpenPuzzleByIndex(int index)
    {
        switch (index)
        {
            case 1:
                if (stormPipePanel != null) stormPipePanel.SetActive(true);
                break;
            case 2:
                if (hazardMiniGamePanel != null) hazardMiniGamePanel.SetActive(true);
                break;
            case 3:
                if (miniGamePanel != null) miniGamePanel.SetActive(true);
                break;
        }
    }

    // --- Puzzle open functions ---
    public void OpenStormPipePuzzle()
    {
        SavePuzzleProgress(1);
        CloseMainPanel();
        if (stormPipePanel != null) stormPipePanel.SetActive(true);
    }

    public void OpenHazardMiniGame()
    {
        SavePuzzleProgress(2);
        CloseMainPanel();
        if (hazardMiniGamePanel != null) hazardMiniGamePanel.SetActive(true);
    }

    public void OpenMiniGamePuzzle()
    {
        SavePuzzleProgress(3);
        CloseMainPanel();
        if (miniGamePanel != null) miniGamePanel.SetActive(true);
    }

    private void SavePuzzleProgress(int index)
    {
        PlayerPrefs.SetInt("StormPuzzleIndex", index);
        PlayerPrefs.SetString("SavedScene", "Storm");
        PlayerPrefs.Save();
    }

    // --- Universal puzzle completion (called by puzzle scripts) ---
    public void PuzzleCompleted(GameObject puzzlePanel)
    {
        if (puzzleDone) return; // already finished, do nothing

        if (puzzlePanel != null) puzzlePanel.SetActive(false);

        // Close panels
        CloseMainPanel();

        puzzleDone = true;
        triggeredOnce = true;

        // ✅ Clear saved puzzle progress since Storm is finished
        PlayerPrefs.DeleteKey("StormPuzzleIndex");
        PlayerPrefs.Save();
    }
}
