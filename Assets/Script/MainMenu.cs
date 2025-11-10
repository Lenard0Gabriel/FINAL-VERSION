using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string[] regularScenes = { "Flood", "Storm", "Fire" };
    private string[] classScenes = { "FloodClass", "FireClass", "EarthquakeClass" };

    void Start()
    {
        Time.timeScale = 1f; // Unpause when menu loads
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        // 0 to 99: 0-69 (70%) for regular, 70-99 (30%) for class scenes
        int chance = Random.Range(0, 100);

        if (chance < 70)
        {
            // 70% chance to load a regular scene
            int randomIndex = Random.Range(0, regularScenes.Length);
            SceneManager.LoadScene(regularScenes[randomIndex]);
        }
        else
        {
            // 30% chance to load a class scene
            int randomIndex = Random.Range(0, classScenes.Length);
            SceneManager.LoadScene(classScenes[randomIndex]);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
