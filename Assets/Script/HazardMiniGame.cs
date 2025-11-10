using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HazardMiniGame : MonoBehaviour, IPointerClickHandler
{
    [Header("Hazards")]
    public Button[] hazardButtons;
    private bool[] hazardFound;

    [Header("UI References")]
    public TextMeshProUGUI feedbackText;

    [Header("Manager Reference")]
    public MegaScenarioManager scenarioManager;

    private int foundCount = 0;

    private void OnEnable()
    {
        foundCount = 0;
        hazardFound = new bool[hazardButtons.Length];
        feedbackText.text = "Find all the hazards!";

        for (int i = 0; i < hazardButtons.Length; i++)
        {
            int index = i;
            hazardButtons[i].onClick.RemoveAllListeners();
            hazardButtons[i].onClick.AddListener(() => OnHazardClicked(index));
        }
    }

    void OnHazardClicked(int index)
    {
        if (!hazardFound[index])
        {
            hazardFound[index] = true;
            foundCount++;
            feedbackText.text = "Good! " + foundCount + "/" + hazardButtons.Length;
            hazardButtons[index].image.color = new Color(0, 1, 0, 0.4f);
        }
        else
        {
            feedbackText.text = "You already clicked this hazard!";
        }

        if (foundCount == hazardButtons.Length)
            Invoke(nameof(FinishMiniGame), 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (System.Array.Exists(hazardButtons, b => b.gameObject == eventData.pointerCurrentRaycast.gameObject))
                return;
        }

        feedbackText.text = "Not a hazard!";
    }

    void FinishMiniGame()
    {
        gameObject.SetActive(false);
        scenarioManager?.PuzzleCompleted(gameObject);
    }
}
