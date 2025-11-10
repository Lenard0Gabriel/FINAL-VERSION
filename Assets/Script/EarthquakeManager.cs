using UnityEngine;
using System.Collections;

public class EarthquakeManager : MonoBehaviour
{
    public Camera mainCamera;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;
    public float aftershockInterval = 5f;
    public PlayerHealth playerHealth;

    private bool isEarthquakeActive = false;
    private bool canTakeDamage = true;
    private float nextAftershockTime;
    private Coroutine shakeCoroutine;

    void Update()
    {
        if (!isEarthquakeActive) return;

        if (Time.time >= nextAftershockTime)
        {
            shakeCoroutine = StartCoroutine(ShakeCamera());
            nextAftershockTime = Time.time + aftershockInterval;
        }
    }

    public void StartEarthquake()
    {
        isEarthquakeActive = true;
        nextAftershockTime = Time.time;
        Debug.Log("🌍 Earthquake started!");
    }

    public void StopEarthquake()
    {
        isEarthquakeActive = false;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        Debug.Log("✅ Earthquake stopped!");
    }

    private IEnumerator ShakeCamera()
    {
        Vector3 originalCamPos = mainCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalCamPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            mainCamera.transform.position = randomPoint;
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCamPos;

        if (playerHealth != null && playerHealth.isHidden == false && canTakeDamage)
        {
            playerHealth.TakeDamage(1);
        }
    }
}
