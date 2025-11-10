using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource1;
    [SerializeField] private AudioSource musicSource2;

    [Header("UI Elements")]
    [SerializeField] private GameObject onUI;   // UI shown when music is ON
    [SerializeField] private GameObject offUI;  // UI shown when music is OFF

    private const string MusicPrefKey = "MusicEnabled";

    private void Start()
    {
        // Apply saved preference (default = ON)
        int musicEnabled = PlayerPrefs.GetInt(MusicPrefKey, 1);

        if (musicEnabled == 1)
            OnMusic(false);  // false = don't overwrite PlayerPrefs again
        else
            OffMusic(false);
    }

    public void ToggleMusic()
    {
        int musicEnabled = PlayerPrefs.GetInt(MusicPrefKey, 1);

        if (musicEnabled == 1)
            OffMusic(true);
        else
            OnMusic(true);
    }

    public void OnMusic(bool save = true)
    {
        if (musicSource1 != null && !musicSource1.isPlaying)
            musicSource1.Play();

        if (musicSource2 != null && !musicSource2.isPlaying)
            musicSource2.Play();

        if (save)
        {
            PlayerPrefs.SetInt(MusicPrefKey, 1);
            PlayerPrefs.Save();
        }

        UpdateUI(true);
    }

    public void OffMusic(bool save = true)
    {
        if (musicSource1 != null && musicSource1.isPlaying)
            musicSource1.Stop();

        if (musicSource2 != null && musicSource2.isPlaying)
            musicSource2.Stop();

        if (save)
        {
            PlayerPrefs.SetInt(MusicPrefKey, 0);
            PlayerPrefs.Save();
        }

        UpdateUI(false);
    }

    private void UpdateUI(bool isOn)
    {
        if (onUI != null) onUI.SetActive(isOn);
        if (offUI != null) offUI.SetActive(!isOn);
    }
}
