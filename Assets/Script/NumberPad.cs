using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.SceneManagement; // <-- Added to load scenes
using System.Collections; // <-- Added for coroutine

public class NumberPad : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI inputText; // Displays entered code
    public TextMeshProUGUI timerText; // Displays time left
    public GameObject gameOverPanel; 
    public GameObject levelCompletePanel;

    [Header("Settings")]
    public string correctCode = "09659168106"; // The correct 11-digit code
    public float timeLimit = 60f; // 1 minute

    private string enteredCode = "";
    private int codeLength = 11;
    private float timeRemaining;
    private bool isGameActive = true;

    void Start()
    {
        timeRemaining = timeLimit;
        inputText.text = "";
        timerText.text = timeRemaining.ToString("F0");
        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameActive)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeRemaining).ToString(); // Show whole seconds

            if (timeRemaining <= 0)
            {
                GameOver();
            }
        }
    }

    // Called by number buttons
    public void NumberPressed(string number)
    {
        if (!isGameActive) return;

        if (enteredCode.Length < codeLength)
        {
            enteredCode += number;
            inputText.text = enteredCode;
        }
    }

    // Called by Backspace button
    public void Backspace()
    {
        if (!isGameActive || enteredCode.Length == 0) return;

        enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);
        inputText.text = enteredCode;
    }

    // Called by Enter button
    public void EnterPressed()
    {
        if (!isGameActive) return;

        if (enteredCode.Length == codeLength)
        {
            CheckCode();
        }
        // If code is not complete, you could give feedback here if you want.
    }

    // Check the entered code
    private void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            LevelComplete();
        }
        else
        {
            enteredCode = "";
            inputText.text = "";
        }
    }

    // Show Game Over and reload scene after 3s
    private void GameOver()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);
        StartCoroutine(ReloadSceneAfterDelay(3f)); // <-- Added
    }

    // Show Level Complete
    private void LevelComplete()
    {
        isGameActive = false;
        levelCompletePanel.SetActive(true);
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("FirePuzzle2"); // <-- Change to your scene name
    }
}
