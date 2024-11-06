using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CompletionSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI[] levelStatusTexts;
    [SerializeField] private Button rewardLinkButton;  // 定义 rewardLinkButton
    [SerializeField] private TextMeshProUGUI rewardButtonText;

    [Header("Button Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color disabledColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    [SerializeField] private string lockedText = "Complete all levels to unlock";
    [SerializeField] private string unlockedText = "Click to claim your reward!";

    private URLButtonOpener urlButtonOpener;

    private void Start()
    {
        if (ValidateComponents())
        {
            urlButtonOpener = rewardLinkButton.GetComponent<URLButtonOpener>();
            if (urlButtonOpener == null)
            {
                Debug.LogError("URLButtonOpener component not found on rewardLinkButton.");
            }

            UpdateLevelStatus();
            UpdateRewardButton();
        }
    }

    private bool ValidateComponents()
    {
        if (levelStatusTexts == null || levelStatusTexts.Length == 0)
        {
            Debug.LogError("Level status texts are not properly configured!");
            return false;
        }

        if (rewardLinkButton == null)
        {
            Debug.LogError("Reward link button is not assigned!");
            return false;
        }

        return true;
    }

    private void UpdateLevelStatus()
    {
        for (int i = 0; i < levelStatusTexts.Length; i++)
        {
            if (levelStatusTexts[i] != null)
            {
                bool isCompleted = LevelCompletionManager.Instance.IsLevelCompleted(i);
                levelStatusTexts[i].text = $"Level - {i + 1} \t\t {(isCompleted ? "1" : "0")}/1";
            }
        }
    }

    private void UpdateRewardButton()
    {
        bool allCompleted = LevelCompletionManager.Instance.AreAllLevelsCompleted();
        SetButtonState(allCompleted);
    }

    private void SetButtonState(bool isEnabled)
    {
        if (urlButtonOpener != null)
        {
            urlButtonOpener.SetInteractable(isEnabled);
        }

        if (rewardButtonText != null)
        {
            rewardButtonText.text = isEnabled ? unlockedText : lockedText;
            rewardButtonText.color = isEnabled ? normalColor : disabledColor;
        }
    }

    public void ResetProgress()
    {
        LevelCompletionManager.Instance.ResetAllProgress();
        UpdateLevelStatus();
        UpdateRewardButton();
    }
}
