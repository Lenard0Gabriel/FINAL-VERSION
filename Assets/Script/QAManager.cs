using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QnAManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctIndex;
    }

    public Question[] questions;

    [Header("UI References")]
    public TextMeshProUGUI questionText; 
    public Button[] answerButtons;
    public TextMeshProUGUI countdownText;
    public GameObject scorePanel; 
    public TextMeshProUGUI scoreText;
    public Button restartButton;
    public GameObject gameOverPanel;

    [Header("Settings")]
    public float timePerQuestion = 10f;
    public int passScore = 3;

    private int currentQuestion = 0;
    private int score = 0;
    private float timer;
    private bool isAnswered = false;

    private Color defaultButtonColor; // store the default button color

    void Start()
    {
        if (answerButtons.Length > 0)
            defaultButtonColor = answerButtons[0].GetComponent<Image>().color;

        scorePanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); 
        if (restartButton != null)
            restartButton.gameObject.SetActive(false); 
        ShowQuestion();
    }

    void Update()
    {
        if (currentQuestion < questions.Length && !isAnswered)
        {
            timer -= Time.deltaTime;

            if (countdownText != null)
                countdownText.text = Mathf.Ceil(timer).ToString();

            if (timer <= 0f)
            {
                isAnswered = true;
                StartCoroutine(NextQuestion(false));
            }
        }
    }

    void ShowQuestion()
    {
        isAnswered = false;
        timer = timePerQuestion;

        Question q = questions[currentQuestion];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < q.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];

                // reset button color
                answerButtons[i].GetComponent<Image>().color = defaultButtonColor;

                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => AnswerQuestion(index));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void AnswerQuestion(int index)
    {
        if (isAnswered) return;
        isAnswered = true;

        bool correct = (index == questions[currentQuestion].correctIndex);
        if (correct) score++;

        // Color feedback:
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i == questions[currentQuestion].correctIndex)
            {
                answerButtons[i].GetComponent<Image>().color = Color.green; // correct = green
            }
            else
            {
                answerButtons[i].GetComponent<Image>().color = Color.red; // wrong = red
            }
        }

        StartCoroutine(NextQuestion(correct));
    }

    IEnumerator NextQuestion(bool correct)
    {
        yield return new WaitForSeconds(1.5f); // give player time to see colors

        currentQuestion++;

        if (currentQuestion < questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            EndQuiz();
        }
    }

    void EndQuiz()
    {
        foreach (var btn in answerButtons)
            btn.gameObject.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (score >= passScore)
        {
            scorePanel.SetActive(true);
            questionText.text = "Quiz Complete!";
            scoreText.text = "Your Score: " + score + " / " + questions.Length;
            if (restartButton != null)
                restartButton.gameObject.SetActive(true);
        }
        else
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
    }

    public void RestartQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
