using UnityEngine;

public class ClickableItems : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.LogWarning("GameManager not found in scene!");
    }

    private void OnMouseDown()
    {
        if (gameManager != null)
            gameManager.CollectItem(this.gameObject);
    }
}
