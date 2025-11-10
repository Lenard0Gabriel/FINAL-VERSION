using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [TextArea]
    public string[] tipMessages; // List of tips

    public GameObject dialogueUI;
    public TMPro.TextMeshProUGUI dialogueText;

    private int tipIndex = 0;

    private void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dialogueUI != null && tipMessages.Length > 0)
        {
            // Show next tip in sequence
            dialogueText.text = tipMessages[tipIndex];

            // Move to the next tip (loop if at the end)
            tipIndex = (tipIndex + 1) % tipMessages.Length;

            dialogueUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }
}
