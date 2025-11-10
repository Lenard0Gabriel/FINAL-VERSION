using UnityEngine;

public class FireInteraction : MonoBehaviour
{
    [Header("Fire Extinguisher Settings")]
    public GameObject sprayLeft;
    public GameObject sprayRight;
    public GameObject extinguisherLeft;
    public GameObject extinguisherRight;
    public float sprayDuration = 3f;

    [Header("References")]
    public MobilePlayerMovement playerMovement; // Reference to movement script
    public FirePuzzleManager puzzleManager;     // Reference to puzzle mission system
    public string fireTag = "Fire";             // Tag for fire objects in scene

    private bool isSpraying = false;
    private float sprayTimer = 0f;

    void Update()
    {
        if (isSpraying)
        {
            UpdateSprayDirection();

            sprayTimer -= Time.deltaTime;
            if (sprayTimer <= 0)
            {
                StopSpray();
            }

            // ✅ Check if all fire is gone while spraying
            if (AllFiresExtinguished())
            {
                StopSpray();
                if (puzzleManager != null)
                {
                    puzzleManager.MissionComplete();
                }
            }
        }
    }

    public void OnSprayPressed()
    {
        if (!isSpraying)
        {
            isSpraying = true;
            sprayTimer = sprayDuration;
            UpdateSprayDirection();
        }
    }

    private void UpdateSprayDirection()
    {
        if (playerMovement != null)
        {
            bool facingRight = playerMovement.FacingRight;

            sprayLeft.SetActive(!facingRight);
            sprayRight.SetActive(facingRight);

            extinguisherLeft.SetActive(!facingRight);
            extinguisherRight.SetActive(facingRight);
        }
    }

    private void StopSpray()
    {
        isSpraying = false;

        sprayLeft.SetActive(false);
        sprayRight.SetActive(false);

        extinguisherLeft.SetActive(false);
        extinguisherRight.SetActive(false);
    }

    // ✅ Check if there are any fire objects left in the scene
    private bool AllFiresExtinguished()
    {
        GameObject[] remainingFires = GameObject.FindGameObjectsWithTag(fireTag);
        return remainingFires.Length == 0;
    }
}
