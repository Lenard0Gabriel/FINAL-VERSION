using UnityEngine;

public class TrashItem : MonoBehaviour
{
    void OnMouseDown()
    {
        // Tell the Game Manager one trash is cleared
        if (DrainageGameManager.Instance != null)
            DrainageGameManager.Instance.TrashCleared();

        // Remove this trash
        Destroy(gameObject);
    }
}
