using UnityEngine;

public class EarthquakeTrigger : MonoBehaviour
{
    private bool hasTriggered = false;
    public EarthquakeManager earthquakeManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Earthquake triggered!");

            if (earthquakeManager != null)
            {
                earthquakeManager.StartEarthquake();
            }
        }
    }
}
