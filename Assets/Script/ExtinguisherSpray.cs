using UnityEngine;

public class ExtinguisherSpray : MonoBehaviour
{
    public GameObject extinguisherObject; // The visible one in hand
    public GameObject sprayObject;        // The smoke/spray animation
    public float sprayDuration = 1f;      // How long spray stays visible

    private bool hasExtinguisher = false;

    public void PickupExtinguisher()
    {
        hasExtinguisher = true;
    }

    public void OnSprayPressed()
    {
        if (!hasExtinguisher) return;

        extinguisherObject.SetActive(true);
        sprayObject.SetActive(true);
        Invoke(nameof(StopSpraying), sprayDuration);
    }

    void StopSpraying()
    {
        extinguisherObject.SetActive(false);
        sprayObject.SetActive(false);
    }
}