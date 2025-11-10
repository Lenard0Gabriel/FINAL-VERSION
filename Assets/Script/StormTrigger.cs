using UnityEngine;

public class StormTrigger : MonoBehaviour
{
    [Header("Assign the storm tutorial panel here")]
    public GameObject stormTutorialPanel;

    private bool hasTriggered = false; // To prevent repeating

    private void Start()
    {
        if (stormTutorialPanel != null)
            stormTutorialPanel.SetActive(false); // Ensure it's hidden initially
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        // Check if player triggered it
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (stormTutorialPanel != null)
            {
                stormTutorialPanel.SetActive(true);
                Time.timeScale = 0f; // Pause the game for tutorial on mobile
            }
        }
    }

    // Call this from your UI Close Button
    public void CloseTutorial()
    {
        if (stormTutorialPanel != null)
        {
            stormTutorialPanel.SetActive(false);
            Time.timeScale = 1f; // Resume game
        }
    }
}
