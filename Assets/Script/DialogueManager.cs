using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialoguePanel;

    [Header("Text Mesh Pro Fields")]
    public TMP_Text dialogueTextMain; // Primary text (e.g., white)
    public TMP_Text dialogueTextShadow; // Secondary text (e.g., black shadow)

    public bool pauseTimeWhenOpen = false;
    public float autoCloseTime = 5f; // Auto close after seconds

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public void Show(string message)
    {
        if (dialoguePanel == null || dialogueTextMain == null || dialogueTextShadow == null)
        {
            Debug.LogWarning("DialogueManager not fully set up.");
            return;
        }

        // Set both text objects
        dialogueTextMain.text = message;
        dialogueTextShadow.text = message;

        dialoguePanel.SetActive(true);

        if (pauseTimeWhenOpen) Time.timeScale = 0f;

        StopAllCoroutines();
        StartCoroutine(AutoCloseAfterDelay());
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(autoCloseTime);
        Close();
    }

    public void Close()
    {
        dialoguePanel.SetActive(false);
        if (pauseTimeWhenOpen) Time.timeScale = 1f;
    }
}