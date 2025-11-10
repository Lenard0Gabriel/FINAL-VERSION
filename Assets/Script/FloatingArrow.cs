using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag
        {
            Destroy(gameObject); // Remove the arrow from the scene
        }
    }
}
