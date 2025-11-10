using UnityEngine;

public class PlayerShelterChecker : MonoBehaviour
{
    [HideInInspector]
    public bool isUnderShelter = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shelter"))
        {
            isUnderShelter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shelter"))
        {
            isUnderShelter = false;
        }
    }
}
