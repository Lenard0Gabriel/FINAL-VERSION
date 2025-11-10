using UnityEngine;

public class ClickableItem : MonoBehaviour
{
    [TextArea] public string message = "Default test message"; // The text shown when clicked

    void OnMouseDown()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.Show(message);
        }
        else
        {
            Debug.LogWarning("No DialogueManager found in scene.");
        }
    }
}