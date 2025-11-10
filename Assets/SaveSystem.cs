using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class ScenePreview
{
    public string sceneName;
    public Sprite previewImage;
}

public class SaveSystem : MonoBehaviour
{
    [Header("Main Menu UI")]
    public Image previewImage;
    public TMP_Text levelNameText;         // white text
    public TMP_Text levelNameShadowText;   // black text (shadow)
    public Button continueButton;

    [Header("Scene Previews")]
    public List<ScenePreview> scenePreviews = new List<ScenePreview>();

    private string saveKey = "SavedScene";

    void Awake()
    {
        // Pre-populate with your 13 levels
        if (scenePreviews.Count == 0)
        {
            scenePreviews.Add(new ScenePreview { sceneName = "Earthquake" });
            scenePreviews.Add(new ScenePreview { sceneName = "Fire" });
            scenePreviews.Add(new ScenePreview { sceneName = "Flood" });
            scenePreviews.Add(new ScenePreview { sceneName = "Storm" });

            scenePreviews.Add(new ScenePreview { sceneName = "CallforHelpScene" });
            scenePreviews.Add(new ScenePreview { sceneName = "FirePuzzle2" });
            scenePreviews.Add(new ScenePreview { sceneName = "FirePuzzle3" });
            scenePreviews.Add(new ScenePreview { sceneName = "FirePuzzleScene" });
            scenePreviews.Add(new ScenePreview { sceneName = "FireQAScene" });
            scenePreviews.Add(new ScenePreview { sceneName = "ReturnFire" });

            scenePreviews.Add(new ScenePreview { sceneName = "BagPuzzleScene" });
            scenePreviews.Add(new ScenePreview { sceneName = "FloodPuzzleScene" });
            scenePreviews.Add(new ScenePreview { sceneName = "QAScene" });
            scenePreviews.Add(new ScenePreview { sceneName = "ReturnFlood" });
        }
    }

    void Start()
    {
        SetupContinuePanel();
    }

    private void SetupContinuePanel()
    {
        string savedScene = PlayerPrefs.GetString(saveKey, "");

        if (!string.IsNullOrEmpty(savedScene))
        {
            // Find matching preview entry
            ScenePreview preview = scenePreviews.Find(x => x.sceneName == savedScene);

            // Update texts
            if (levelNameText != null)
                levelNameText.text = savedScene;
            if (levelNameShadowText != null)
                levelNameShadowText.text = savedScene;

            // Update image (if you assign sprites in Inspector later)
            if (previewImage != null && preview != null && preview.previewImage != null)
            {
                previewImage.sprite = preview.previewImage;
                previewImage.enabled = true;
            }
            else if (previewImage != null)
            {
                previewImage.enabled = false;
            }

            // Enable Continue button
            if (continueButton != null)
            {
                continueButton.interactable = true;
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(() => SceneManager.LoadScene(savedScene));
            }
        }
        else
        {
            // No save found
            if (levelNameText != null) levelNameText.text = "No Save Found";
            if (levelNameShadowText != null) levelNameShadowText.text = "No Save Found";
            if (previewImage != null) previewImage.enabled = false;
            if (continueButton != null) continueButton.interactable = false;
        }
    }
}
