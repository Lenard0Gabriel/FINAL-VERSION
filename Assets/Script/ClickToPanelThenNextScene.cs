using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToChangeScene : MonoBehaviour
{
    void OnMouseDown()
    {
        StartCoroutine(ChangeSceneWithDelay());
    }

    private System.Collections.IEnumerator ChangeSceneWithDelay()
    {
        yield return new WaitForSeconds(3f); // wait 3 seconds
        SceneManager.LoadScene("CallforHelpScene");
    }
}
