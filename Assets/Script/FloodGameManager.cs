using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FloodGameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;       // Assign TextMeshPro for timer (leave initial text blank)
    public TMP_Text messageText;     // Assign TextMeshPro for feedback (leave initial text blank)

    [Header("Game Settings")]
    public float timeLimit = 30f;           // Countdown seconds
    public string[] requiredItems;          // Names of correct items (must match GameObject.name)
    public float messageDuration = 1.5f;    // seconds feedback stays visible

    [Header("Bag Movement")]
    public Transform bagTransform;          // Assign the Bag transform (world-space) in Inspector
    public float moveDuration = 0.35f;      // How long the object takes to move to bag (seconds, uses unscaled time)

    private float timer;
    private HashSet<string> collectedItems = new HashSet<string>();
    private bool gameActive = false;
    private float messageTimer = 0f;

    void Start()
    {
        timer = timeLimit;
        if (messageText != null) messageText.text = "";
        if (timerText != null) timerText.text = Mathf.Ceil(timer).ToString();
    }

    void Update()
    {
        // Timer runs only if game is active
        if (gameActive)
        {
            timer -= Time.deltaTime;
            if (timerText != null)
                timerText.text = Mathf.Ceil(timer).ToString();

            if (timer <= 0f)
                EndGame(false);
        }

        // Message auto-hide (note: if you set Time.timeScale = 0, this will stop)
        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f && messageText != null)
                messageText.text = "";
        }
    }

    // Call this to start the puzzle/timer
    public void StartPuzzle()
    {
        collectedItems.Clear();
        timer = timeLimit;
        gameActive = true;
        if (timerText != null) timerText.text = Mathf.Ceil(timer).ToString();
        if (messageText != null) messageText.text = "";
    }

    // Called by ItemClick when an item is tapped
    public void CollectItemByClick(GameObject item)
    {
        if (!gameActive || item == null) return;

        string itemName = item.name;
        bool isRequired = IsRequiredItem(itemName);

        if (isRequired)
        {
            if (!collectedItems.Contains(itemName))
            {
                // register immediately so repeated taps don't double-count
                collectedItems.Add(itemName);

                // move item to bag visually, then disable when reached
                StartCoroutine(MoveItemToBag(item));

                ShowMessage("Picked up: " + itemName);

                if (collectedItems.Count == requiredItems.Length)
                {
                    // Delay EndGame a fraction if you want item animation to finish first.
                    // We'll EndGame shortly after — but movement uses unscaled time so it won't freeze.
                    EndGame(true);
                }
            }
            else
            {
                ShowMessage("Already picked: " + itemName);
            }
        }
        else
        {
            WrongItem(itemName);
        }
    }

    // Check if a name is in the requiredItems list
    private bool IsRequiredItem(string name)
    {
        if (requiredItems == null) return false;
        for (int i = 0; i < requiredItems.Length; i++)
            if (requiredItems[i] == name) return true;
        return false;
    }

    public void WrongItem(string itemName)
    {
        ShowMessage("Wrong item!");
        timer -= 3f; // penalty
        if (timer < 0f) timer = 0f;
    }

    // Coroutine: move clicked item to bag position using unscaled time (so it still moves if timeScale changes)
    private IEnumerator MoveItemToBag(GameObject item)
    {
        if (item == null) yield break;

        // disable collider so it can't be clicked again while moving
        Collider2D col = item.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Vector3 start = item.transform.position;
        Vector3 target = (bagTransform != null) ? bagTransform.position : start;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            if (item == null) yield break;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            item.transform.position = Vector3.Lerp(start, target, t);
            elapsed += Time.unscaledDeltaTime; // use unscaled so animation runs even if timescale is changed elsewhere
            yield return null;
        }

        // ensure final pos and then hide item
        if (item != null)
        {
            item.transform.position = target;
            item.SetActive(false);
        }
    }

    // Show feedback text for messageDuration seconds
    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageTimer = messageDuration;
        }
        else
        {
            Debug.Log(message);
        }
    }

    // Public wrapper in case other scripts call UpdateMessage
    public void UpdateMessage(string message)
    {
        ShowMessage(message);
    }

    private void EndGame(bool won)
    {
        gameActive = false;

        if (won) ShowMessage("Mission Complete!");
        else ShowMessage("Time's Up! You missed items!");

        // freeze gameplay (optional)
        Time.timeScale = 0f;
    }
}
