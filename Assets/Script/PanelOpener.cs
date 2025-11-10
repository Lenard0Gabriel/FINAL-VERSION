using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panelToShow; // Drag your panel here in the Inspector

    void OnMouseDown()
    {
        // Don't allow interaction if another panel is already open
        if (HotlinePanel.panelOpen) return;

        if (panelToShow != null)
        {
            bool isActive = panelToShow.activeSelf;
            panelToShow.SetActive(!isActive);

            if (!isActive)
            {
                // Mark as open and disable all clickables
                HotlinePanel.panelOpen = true;
                foreach (var clickable in FindObjectsOfType<Collider2D>())
                    clickable.enabled = false;
            }
            else
            {
                // Mark as closed and re-enable clickables
                HotlinePanel.panelOpen = false;
                foreach (var clickable in FindObjectsOfType<Collider2D>())
                    clickable.enabled = true;
            }
        }
    }
}
