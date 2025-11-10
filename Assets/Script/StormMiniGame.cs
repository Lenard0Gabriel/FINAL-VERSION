using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StormMiniGame : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;    // must be size 3
        public int correctIndex;    // which answer is correct
    }

    [Header("Mini Game Settings")]
    public Question[] questions;
    private int currentQuestion = 0;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;

    [Header("Timer Settings")]
    public float timeLimit = 30f; // total time in seconds
    private float timeRemaining;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;

    [Header("Manager Reference")]
    public MegaScenarioManager scenarioManager;

    private bool gameActive = false;

    private void OnEnable()
    {
        currentQuestion = 0;
        LoadQuestion();

        // Reset timer
        timeRemaining = timeLimit;
        gameActive = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    private void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
            timeRemaining = 0;

        if (timerText != null)
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString(); // only the number

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    void LoadQuestion()
    {
        feedbackText.text = "";
        Question q = questions[currentQuestion];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];

            // Reset button color (solid white) and re-enable interaction
            answerButtons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            answerButtons[i].interactable = true;

            // Clear old listeners then add new one
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    void OnAnswerSelected(int index)
    {
        // Disable further interaction once an answer is picked
        foreach (var button in answerButtons)
            button.interactable = false;

        // Correct answer chosen
        if (index == questions[currentQuestion].correctIndex)
        {
            feedbackText.text = "Correct!";
            answerButtons[index].GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f); // solid green
            Invoke(nameof(NextStep), 1f);
        }
        else
        {
            feedbackText.text = "Oops! Try again!";

            // Highlight correct answer solid green
            answerButtons[questions[currentQuestion].correctIndex].GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);

            // Highlight all wrong answers solid red
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (i != questions[currentQuestion].correctIndex)
                    answerButtons[i].GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
            }

            // Move to next step after short delay
            Invoke(nameof(NextStep), 1.5f);
        }
    }

    void NextStep()
    {
        currentQuestion++;
        if (currentQuestion < questions.Length)
            LoadQuestion();
        else
            Invoke(nameof(FinishMiniGame), 1f);
    }

    void FinishMiniGame()
    {
        gameActive = false;
        gameObject.SetActive(false);
        scenarioManager?.PuzzleCompleted(gameObject);
    }

    void GameOver()
    {
        gameActive = false;

        // disable interaction
        foreach (var button in answerButtons)
            button.interactable = false;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }
}
