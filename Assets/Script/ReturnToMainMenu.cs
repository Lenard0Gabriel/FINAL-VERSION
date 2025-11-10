using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        if (ChallengeManager.Instance != null)
        {
            ChallengeManager.Instance.ResetManager();
        }

        SceneManager.LoadScene("MainMenu");
    }
}
