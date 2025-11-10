using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneMusicPlayer : MonoBehaviour
{
    public AudioClip musicClip;

    private AudioSource audioSource;

    void Start()
    {
        // Automatically destroy this music player if loaded in MainMenu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (musicClip == null)
        {
            Debug.LogWarning("SceneMusicPlayer: No music clip assigned.");
            return;
        }

        audioSource.clip = musicClip;
        audioSource.volume = 0.2f; // 20% volume
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }
}
