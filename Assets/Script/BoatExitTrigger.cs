using UnityEngine;
using System.Collections;

public class BoatExitTrigger : MonoBehaviour
{
    public GameObject missionCompletePanel;
    public Transform boat;
    public Transform player;
    public float boatSpeed = 2f;
    public Vector3 boatExitDirection = new Vector3(1f, 0f, 0f);
    public float boatMoveTime = 3f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(HandleExitSequence());
        }
    }

    IEnumerator HandleExitSequence()
    {
        // Disable player movement if possible
        if (player != null)
        {
            var movementScript = player.GetComponent<MobilePlayerMovement>();
            if (movementScript != null)
                movementScript.enabled = false;

            player.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ Player reference is missing in BoatExitTrigger.");
        }

        // Set camera to follow the boat
        if (boat != null)
        {
            var cameraFollow = Camera.main?.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(boat);
            }
            else
            {
                Debug.LogWarning("⚠️ CameraFollow script not found on main camera.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Boat reference is missing in BoatExitTrigger.");
        }

        yield return new WaitForSeconds(0.5f);

        // Move the boat
        if (boat != null)
        {
            float timer = 0f;
            while (timer < boatMoveTime)
            {
                boat.Translate(boatExitDirection * boatSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        // Show mission complete panel
        if (missionCompletePanel != null)
        {
            missionCompletePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ MissionCompletePanel reference is missing in BoatExitTrigger.");
        }
    }
}
