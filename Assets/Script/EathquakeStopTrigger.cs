using UnityEngine;

public class EarthquakeStopTrigger : MonoBehaviour
{
    public EarthquakeManager earthquakeManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish line reached. Stopping earthquake.");
            if (earthquakeManager != null)
            {
                earthquakeManager.StopEarthquake();
            }
        }
    }
}
