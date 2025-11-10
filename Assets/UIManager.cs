using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathPanel;
    public TextMeshProUGUI deathReasonText;
    public float messageDuration = 3f;

    [Header("Death Tips")]
    [TextArea(2, 4)]
    public string[] deathTips = new string[]
    {
        "💡 You were hit by falling garbage!",
        "💡 Try to dodge next time!",
        "💡 Watch for garbage falls where they appear!"
    };

    public void ShowDeathReason()
    {
        if (deathReasonText == null) return;

        // Pick random tip
        string randomTip = "";
        if (deathTips != null && deathTips.Length > 0)
        {
            int index = Random.Range(0, deathTips.Length);
            randomTip = deathTips[index];
        }

        // Show UI
        if (deathPanel != null)
            deathPanel.SetActive(true);

        deathReasonText.text = randomTip;
        deathReasonText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideDeathReason));
        Invoke(nameof(HideDeathReason), messageDuration);
    }

    private void HideDeathReason()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (deathReasonText != null)
            deathReasonText.gameObject.SetActive(false);
    }
}
