using UnityEngine;
using UnityEngine.UI;

public class PASSManager : MonoBehaviour
{
    [Header("PASS Buttons")]
    public Button pullButton;
    public Button aimButton;
    public Button squeezeButton;
    public Button sweepButton;

    [Header("Mission Panels")]
    public GameObject missionCompleteUI;
    public GameObject missionFailedUI;

    [Header("Fire Extinguisher Parts")]
    public Transform pinObject;           // Drag the pin GameObject here
    public Transform pinPulledPosition;   // Drag an empty GameObject that marks the target position
    public float pinMoveSpeed = 3f;       // Speed (units per second, adjustable in Inspector)

    private int step = 0;
    private static PASSManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (pullButton != null) pullButton.onClick.AddListener(() => CheckStep(0, pullButton));
        if (aimButton != null) aimButton.onClick.AddListener(() => CheckStep(1, aimButton));
        if (squeezeButton != null) squeezeButton.onClick.AddListener(() => CheckStep(2, squeezeButton));
        if (sweepButton != null) sweepButton.onClick.AddListener(() => CheckStep(3, sweepButton));

        if (missionCompleteUI != null) missionCompleteUI.SetActive(false);
        if (missionFailedUI != null) missionFailedUI.SetActive(false);
    }

    void CheckStep(int buttonStep, Button clickedButton)
    {
        if (buttonStep == step)
        {
            // Hide button when pressed correctly
            if (clickedButton != null)
                clickedButton.gameObject.SetActive(false);

            // Special action for Pull step
            if (buttonStep == 0 && pinObject != null && pinPulledPosition != null)
            {
                // Smoothly move the pin at a controlled speed
                StopAllCoroutines(); // Stop any previous movement if it was still running
                StartCoroutine(MovePin(pinObject, pinPulledPosition.position, pinMoveSpeed));
            }

            step++;
            if (step > 3)
            {
                ShowMissionComplete();
            }
        }
        else
        {
            ShowMissionFailed();
        }
    }

    void ShowMissionComplete()
    {
        CloseTutorialPanel();
        if (missionCompleteUI != null)
            missionCompleteUI.SetActive(true);
    }

    void ShowMissionFailed()
    {
        CloseTutorialPanel();
        if (missionFailedUI != null)
            missionFailedUI.SetActive(true);
    }

    public void CloseTutorialPanel()
    {
        gameObject.SetActive(false);
        FireExtinguisherClick.tutorialOpen = false;
        Time.timeScale = 1f;

        foreach (var clickable in FindObjectsOfType<Collider2D>())
        {
            clickable.enabled = true;
        }
    }

    public static void ForceCloseIfOpen()
    {
        if (instance != null && instance.gameObject.activeSelf)
        {
            instance.CloseTutorialPanel();
        }
    }

    private System.Collections.IEnumerator MovePin(Transform pin, Vector3 target, float speed)
    {
        while (Vector3.Distance(pin.position, target) > 0.01f)
        {
            pin.position = Vector3.MoveTowards(pin.position, target, speed * Time.unscaledDeltaTime);
            yield return null;
        }
        pin.position = target; // ensure it snaps exactly at end
    }
}
