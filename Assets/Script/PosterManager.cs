using UnityEngine;
using TMPro;

public class PosterManager : MonoBehaviour
{
    public static PosterManager Instance;

    public GameObject posterPanel;    // Assign in inspector
    public TMP_Text posterText;       // Assign in inspector

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (posterPanel != null) posterPanel.SetActive(false);
    }

    public void ShowPoster(string message)
    {
        if (posterPanel == null || posterText == null)
        {
            Debug.LogWarning("PosterManager not fully set up.");
            return;
        }

        posterText.text = message;
        posterPanel.SetActive(true);
        Time.timeScale = 0f; // Optional: pause game while reading
    }

    public void ClosePoster()
    {
        if (posterPanel != null) posterPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game if paused
    }
}
