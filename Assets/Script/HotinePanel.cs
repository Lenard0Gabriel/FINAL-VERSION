using UnityEngine;

public class HotlinePanel : MonoBehaviour
{
    [Header("Assign all your panels here")]
    public GameObject[] panels;  // Drag and drop multiple panels into this array in the Inspector

    public static bool panelOpen = false; // Global flag to block clicks
    private static GameObject currentPanel = null;

    private void Start()
    {
        // Make sure all panels start hidden
        foreach (var panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        panelOpen = false;
        currentPanel = null;
    }

    // Open a specific panel by index
    public void OpenPanel(int index)
    {
        if (panelOpen) return; // Prevent opening if another panel is open

        if (index >= 0 && index < panels.Length && panels[index] != null)
        {
            panels[index].SetActive(true);
            panelOpen = true;
            currentPanel = panels[index];

            // Disable all other clickable items
            foreach (var clickable in FindObjectsOfType<Collider2D>())
                clickable.enabled = false;
        }
    }

    // Close a specific panel by index
    public void ClosePanel(int index)
    {
        if (index >= 0 && index < panels.Length && panels[index] != null)
        {
            panels[index].SetActive(false);

            // Reset panel flag if this is the one that was open
            if (currentPanel == panels[index])
            {
                panelOpen = false;
                currentPanel = null;

                // Re-enable all clickable items
                foreach (var clickable in FindObjectsOfType<Collider2D>())
                    clickable.enabled = true;
            }
        }
    }

    // Close all panels at once (optional helper)
    public void CloseAllPanels()
    {
        foreach (var panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        panelOpen = false;
        currentPanel = null;

        foreach (var clickable in FindObjectsOfType<Collider2D>())
            clickable.enabled = true;
    }
}
