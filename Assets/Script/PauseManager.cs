using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Assign in inspector

    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Freeze game
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Resume game
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time before quitting
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
