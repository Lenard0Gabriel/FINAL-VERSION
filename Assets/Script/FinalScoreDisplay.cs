using UnityEngine;
using TMPro; // Only if you're using TextMeshPro

public class FinalScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        scoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;
    }
}
