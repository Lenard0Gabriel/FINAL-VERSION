using UnityEngine;
using UnityEngine.UI;

public class PipePiece : MonoBehaviour
{
    public Button button;                  // assign in Inspector
    public Image image;                    // optional for visual feedback
    public float[] correctRotations;       // allowed rotations
    public bool isCorrect = false;

    [HideInInspector]
    public MegaScenarioManager scenarioManager; // assigned by MegaTrigger
    [HideInInspector]
    public GameObject puzzlePanel;           // assigned by MegaTrigger

    private bool locked = false; // lock once correct

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(RotatePipe);
    }

    void RotatePipe()
    {
        if (locked) return; // do nothing if locked

        transform.Rotate(0, 0, -90f); // rotate 90 degrees
        CheckCorrectRotation();

        if (isCorrect)
        {
            locked = true; // lock pipe in place
        }

        // Check all pipes in this puzzle
        PipePiece[] allPipes = puzzlePanel.GetComponentsInChildren<PipePiece>();
        bool allCorrect = true;
        foreach (PipePiece p in allPipes)
        {
            if (!p.isCorrect)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            // all pipes are correct → notify MegaTrigger
            scenarioManager?.PuzzleCompleted(puzzlePanel);
        }
    }

    public void CheckCorrectRotation()
    {
        float currentZ = Mathf.Round(transform.eulerAngles.z) % 360;
        isCorrect = false;

        foreach (float validAngle in correctRotations)
        {
            float normalized = (validAngle + 360) % 360;
            if (Mathf.Approximately(currentZ, normalized))
            {
                isCorrect = true;
                break;
            }
        }

        // optional visual feedback
        if (image != null)
            image.color = isCorrect ? Color.green : Color.white;
    }
}
