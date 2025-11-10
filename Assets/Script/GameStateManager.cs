using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public Vector3 playerPosition;
    public float savedTimeScale = 1f;
    public string returnScene = "Fire";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(Transform player)
    {
        playerPosition = player.position;
        savedTimeScale = Time.timeScale;
    }

    public void LoadPlayerState(GameObject player)
    {
        player.transform.position = playerPosition;
        Time.timeScale = savedTimeScale;
    }

    public void GoToPuzzleScene()
    {
        SceneManager.LoadScene("FirePuzzleScene");
    }

    public void ReturnToFireScene()
    {
        SceneManager.LoadScene(returnScene);
    }
}
