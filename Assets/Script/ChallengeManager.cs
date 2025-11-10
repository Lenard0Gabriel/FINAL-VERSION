using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance;

    private Queue<string> sceneQueue = new Queue<string>();
    public int currentScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // ✅ Always reset timescale on new scene load
        Time.timeScale = 1f;
    }

    public void StartChallenge()
    {
        currentScore = 0;
        QueueScenesInOrder();
        LoadNextScene();
    }

    private void QueueScenesInOrder()
    {
        sceneQueue.Clear();

        sceneQueue.Enqueue("A1");
        sceneQueue.Enqueue("A2");
        sceneQueue.Enqueue("A4");
        sceneQueue.Enqueue("A3");

        Debug.Log("Ordered scenes: " + string.Join(", ", sceneQueue));
    }

    public void LoadNextScene()
    {
        ResetTimeScale(); // ✅ Ensure game isn’t paused

        if (sceneQueue.Count > 0)
        {
            string nextScene = sceneQueue.Dequeue();
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene("ChallengeSummaryScene");
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public void ResetManager()
    {
        Instance = null;
        Destroy(gameObject);
    }

    private void ResetTimeScale()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f; // ✅ Prevents “stuck pause”
    }
}
