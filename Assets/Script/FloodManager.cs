using UnityEngine;

public class FloodManager : MonoBehaviour
{
    public Transform water; // still reference to water if needed
    public float riseSpeed = 0f; // now irrelevant
    public float delayBeforeStart = 0f; // unused
    public float targetY = 0f; // unused

    void Update()
    {
        // Do nothing. Rising is disabled.
    }
}
