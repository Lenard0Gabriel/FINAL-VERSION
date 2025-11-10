using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // 🔹 Added for screenshot coroutine
using System.IO;          // 🔹 For saving image file

public class GameOverManager : MonoBehaviour
{
    // ✅ Singleton so PlayerHealth can call GameOverManager.Instance
    public static GameOverManager Instance { get; private set; }

    public GameObject gameOverPanel;
    public GameObject scorePanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;

    private float levelStartTime;
    private int maxScorePerLevel = 100;

    // Fixed scene order for challenge mode
    private string[] challengeScenes = { "A1", "A2", "A4", "A3" }; // A3 = final stage

    // 🔹 Added for Save System
    private string saveKey = "SavedScene";
    private string imagePath;

    private void Awake()
    {
        // Setup singleton
        if (Instance == null)
        {
            Instance = this;
            // Optional: keep this manager across scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Always unpause on load
        Time.timeScale = 1f;

        // Ensure panels start hidden
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(false);

        // 🔹 Save System path
        imagePath = Application.persistentDataPath + "/savePreview.png";
    }

    private void Start()
    {
        levelStartTime = Time.time;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        ResetTimeScale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void GoToMainMenu()
    {
        ResetTimeScale();

        // 🔹 Save scene before going back to Main Menu
        SaveCurrentScene();

        SceneManager.LoadScene("MainMenu");
    }

    public void GoToNextChallenge()
    {
        ResetTimeScale();

        CalculateAndSaveScore();

        string currentScene = SceneManager.GetActiveScene().name;
        int currentIndex = System.Array.IndexOf(challengeScenes, currentScene);

        if (currentIndex >= 0 && currentIndex < challengeScenes.Length - 1)
        {
            string nextScene = challengeScenes[currentIndex + 1];
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            ShowScorePanel();
        }
    }

    private void ResetTimeScale()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }

    private void CalculateAndSaveScore()
    {
        float timeTaken = Time.time - levelStartTime;
        int earnedScore = Mathf.Clamp(Mathf.RoundToInt(maxScorePerLevel - timeTaken), 0, maxScorePerLevel);

        int currentTotalScore = PlayerPrefs.GetInt("Score", 0);
        currentTotalScore += earnedScore;
        PlayerPrefs.SetInt("Score", currentTotalScore);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentTotalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentTotalScore);
        }

        PlayerPrefs.Save();
    }

    public void ShowScorePanel()
    {
        ResetTimeScale();

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "A3") return;

        if (scorePanel != null)
        {
            scorePanel.SetActive(true);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }

        int score = PlayerPrefs.GetInt("Score", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }

    public void ExitFromFinalScore()
    {
        PlayerPrefs.DeleteKey("Score");
        GoToMainMenu();
    }

    // 🔹 ---------------- SAVE SYSTEM FUNCTIONS ----------------
    private void SaveCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Save scene name
        PlayerPrefs.SetString(saveKey, sceneName);
        PlayerPrefs.Save();

        Debug.Log("Game Saved: " + sceneName);

        // Save screenshot for preview
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(imagePath, tex.EncodeToPNG());
        Debug.Log("Saved preview at: " + imagePath);

        Destroy(tex);
    }
}
