using UnityEngine;

public class FireExtinguisherClick : MonoBehaviour
{
    public GameObject tutorialPanel;
    public static bool tutorialOpen = false;

    private static GameObject currentTutorialPanel;

    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        if (tutorialOpen) return; // Prevent opening twice

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            tutorialOpen = true;
            currentTutorialPanel = tutorialPanel;

            // Disable raycasts for everything else
            foreach (var clickable in FindObjectsOfType<Collider2D>())
            {
                clickable.enabled = false;
            }
        }
    }

    // Closes the tutorial panel completely
    public static void CloseTutorial()
    {
        tutorialOpen = false;

        if (currentTutorialPanel != null)
            currentTutorialPanel.SetActive(false);

        // Re-enable raycasts
        foreach (var clickable in FindObjectsOfType<Collider2D>())
        {
            clickable.enabled = true;
        }
    }
}
