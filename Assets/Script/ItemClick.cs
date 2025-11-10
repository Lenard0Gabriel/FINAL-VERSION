using UnityEngine;

public class ItemClick : MonoBehaviour
{
    private FloodGameManager floodManager;

    private void Start()
    {
        floodManager = FindObjectOfType<FloodGameManager>();
        if (floodManager == null)
            Debug.LogWarning("FloodGameManager not found in scene. Make sure a FloodGameManager exists.");
    }

    private void Update()
    {
        // Touch input for mobile
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(t.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    OnItemClicked();
                }
            }
        }

        // Mouse input for editor/testing
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OnItemClicked();
            }
        }
#endif
    }

    private void OnItemClicked()
    {
        if (floodManager != null)
        {
            floodManager.CollectItemByClick(gameObject);
            // Do not immediately disable here — manager will animate and disable correct items.
        }
        else
        {
            Debug.LogWarning("FloodGameManager missing; can't process click.");
        }
    }
}
