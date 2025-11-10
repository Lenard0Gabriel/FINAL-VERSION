using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class QuestionData
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex; // 0, 1, 2, 3
}

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text questionText;
    public Button[] answerButtons;
    public GameObject resultPanel;
    public TMP_Text resultText;

    [Header("Quiz Data")]
    public QuestionData[] questions;
    private int currentQuestionIndex = 0;
    private int score = 0;

    private void Start()
    {
        resultPanel.SetActive(false);
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        QuestionData q = questions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            btnText.text = q.answers[i];

            int buttonIndex = i; // Local copy for closure
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(buttonIndex));
        }
    }

    void CheckAnswer(int index)
    {
        if (index == questions[currentQuestionIndex].correctAnswerIndex)
            score++;

        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Length)
            DisplayQuestion();
        else
            ShowResult();
    }

    void ShowResult()
    {
        questionText.gameObject.SetActive(false);
        foreach (Button btn in answerButtons)
            btn.gameObject.SetActive(false);

        resultPanel.SetActive(true);
        resultText.text = $"You got {score} out of {questions.Length} correct!";
    }

    public void RetryQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Flood"); // Change to your main scene name
    }
}
