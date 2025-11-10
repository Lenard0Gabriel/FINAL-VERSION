using UnityEngine;
using UnityEngine.SceneManagement;

public class DisasterLoadScene : MonoBehaviour
{
    private void ResetTimeScale()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f; // ✅ Prevent loading scene while paused
    }

    public void LoadFlood()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Flood");
    }

    public void LoadStorm()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Storm");
    }

    public void LoadEarthquake()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Earthquake");
    }

    public void LoadFire()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Fire");
    }

    public void LoadEarthquakeClass()
    {
        ResetTimeScale();
        SceneManager.LoadScene("EarthquakeClass");
    }

    public void LoadFloodClass()
    {
        ResetTimeScale();
        SceneManager.LoadScene("FloodClass");
    }

    public void LoadFireClass()
    {
        ResetTimeScale();
        SceneManager.LoadScene("FireClass");
    }
}
