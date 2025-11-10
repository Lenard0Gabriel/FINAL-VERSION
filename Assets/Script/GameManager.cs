using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform bagTransform;       // Target position for correct items
    public Text timerText;               // Countdown display
    public GameObject missionCompleteUI;
    public GameObject missionFailedUI;

    [Header("Game Settings")]
    public float timeLimit = 120f;       // e.g. 2 minutes
    public GameObject[] clickableItems;  // Include both correct and wrong items here
    public float moveDuration = 0.5f;
    public float timePenalty = 10f;      // Lose 10 seconds per wrong click

    private HashSet<GameObject> collectedItems = new HashSet<GameObject>();
    private int totalCorrectItems = 0;
    private float timer;
    private bool gameActive = false;

    private void Start()
    {
        // Count correct items by tag
        foreach (var obj in clickableItems)
        {
            if (obj.CompareTag("Correct"))
                totalCorrectItems++;
        }

        timer = timeLimit;
        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString();

        if (missionCompleteUI != null) missionCompleteUI.SetActive(false);
        if (missionFailedUI != null) missionFailedUI.SetActive(false);

        gameActive = true;
    }

    private void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString();

        if (timer <= 0f)
        {
            EndGame(false);
        }
    }

    public void CollectItem(GameObject item)
    {
        if (!gameActive || collectedItems.Contains(item)) return;

        collectedItems.Add(item);

        if (item.CompareTag("Correct"))
        {
            StartCoroutine(MoveItemToBag(item));

            // Check if all correct items are collected
            int collectedCorrect = 0;
            foreach (var obj in collectedItems)
            {
                if (obj.CompareTag("Correct"))
                    collectedCorrect++;
            }

            if (collectedCorrect >= totalCorrectItems)
            {
                EndGame(true);
            }
        }
        else if (item.CompareTag("Wrong"))
        {
            // Apply time penalty
            timer -= timePenalty;
            if (timer < 0) timer = 0;

            // Optional: add visual feedback (e.g., flash red)
            StartCoroutine(FlashRed(item));

            // Just disable wrong object so it can't be clicked again
            item.SetActive(false);
        }
    }

    private IEnumerator MoveItemToBag(GameObject item)
    {
        Vector3 start = item.transform.position;
        Vector3 target = bagTransform.position;
        float elapsed = 0f;

        // Disable collider so it can't be clicked again
        Collider2D col = item.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        while (elapsed < moveDuration)
        {
            item.transform.position = Vector3.Lerp(start, target, elapsed / moveDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        item.transform.position = target;
        item.SetActive(false);
    }

    private IEnumerator FlashRed(GameObject item)
    {
        SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color original = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = original;
        }
    }

    private void EndGame(bool won)
    {
        gameActive = false;
        if (won)
        {
            if (missionCompleteUI != null) missionCompleteUI.SetActive(true);
        }
        else
        {
            if (missionFailedUI != null) missionFailedUI.SetActive(true);
        }
    }
}
