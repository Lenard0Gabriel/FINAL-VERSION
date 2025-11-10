using UnityEngine;
using UnityEngine.UI;

public class DrainageGameManager : MonoBehaviour
{
    public static DrainageGameManager Instance;

    public int totalTrash;
    private int cleared = 0;

    public float timer = 20f;
    public Text timerText;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Tips UI")]
    public Text tipText1;
    public Text tipText2;

    private bool gameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Count trash at start
        totalTrash = GameObject.FindObjectsOfType<TrashItem>().Length;

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);

        UpdateTimerUI();
        ShowTips();
    }

    void Update()
    {
        if (gameOver) return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0 && cleared < totalTrash)
        {
            LoseGame();
        }
    }

    public void TrashCleared()
    {
        cleared++;
        if (cleared >= totalTrash)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameOver = true;
        if (winPanel) winPanel.SetActive(true);
    }

    void LoseGame()
    {
        gameOver = true;
        if (losePanel) losePanel.SetActive(true);
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(Mathf.Max(timer, 0)).ToString();
        }
    }

    void ShowTips()
    {
        if (tipText1 != null)
            tipText1.text = "Click the trash to clear the drainage.";
        if (tipText2 != null)
            tipText2.text = "Remove everything before time runs out!";
    }
}
