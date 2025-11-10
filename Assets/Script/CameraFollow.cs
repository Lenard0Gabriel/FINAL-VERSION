using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Follow Settings")]
    public Transform target; // Will auto-assign if left null
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [Header("Camera Bounds")]
    public float areaWidth = 40f;
    public float areaHeight = 10f;
    public Vector2 centerPosition = Vector2.zero;

    private Vector2 minPosition;
    private Vector2 maxPosition;

    private void Start()
    {
        float halfWidth = areaWidth / 2f;
        float halfHeight = areaHeight / 2f;

        minPosition = new Vector2(centerPosition.x - halfWidth, centerPosition.y - halfHeight);
        maxPosition = new Vector2(centerPosition.x + halfWidth, centerPosition.y + halfHeight);

        // Auto-find player when the scene starts
        FindPlayer();
    }

    private void LateUpdate()
    {
        // If target is gone (scene reload, object destroyed), re-find player
        if (target == null)
        {
            FindPlayer();
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(areaWidth, areaHeight, 0f);
        Gizmos.DrawWireCube(centerPosition, size);
    }
}
