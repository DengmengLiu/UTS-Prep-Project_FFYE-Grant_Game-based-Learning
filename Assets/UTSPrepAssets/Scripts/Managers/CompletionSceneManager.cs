using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CompletionSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI[] levelStatusTexts;
    [SerializeField] private Button rewardLinkButton;
    [SerializeField] private TextMeshProUGUI rewardButtonText;
    
    [Header("Settings")]
    [SerializeField] private int totalLevels = 4;
    [SerializeField] private string rewardURL = "https://example.com";
    
    [Header("Button Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color disabledColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    [SerializeField] private string lockedText = "Complete all levels to unlock";
    [SerializeField] private string unlockedText = "Click to claim your reward!";

    private void Start()
    {
        if (ValidateComponents())
        {
            UpdateLevelStatus();
            UpdateRewardButton();
        }
    }

    private bool ValidateComponents()
    {
        if (levelStatusTexts == null || levelStatusTexts.Length < totalLevels)
        {
            Debug.LogError($"Level status texts not properly configured! Expected {totalLevels} texts.");
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
        for (int i = 0; i < totalLevels && i < levelStatusTexts.Length; i++)
        {
            if (levelStatusTexts[i] != null)
            {
                bool isCompleted = LevelCompletionManager.Instance.IsLevelCompleted(i);
                levelStatusTexts[i].text = $"Level - {i + 1} {(isCompleted ? "1" : "0")}/1";
            }
        }
    }

    private void UpdateRewardButton()
    {
        bool allCompleted = LevelCompletionManager.Instance.AreAllLevelsCompleted();
        
        // 更新按钮交互状态
        rewardLinkButton.interactable = allCompleted;
        
        // 更新按钮文本
        if (rewardButtonText != null)
        {
            rewardButtonText.text = allCompleted ? unlockedText : lockedText;
            rewardButtonText.color = allCompleted ? normalColor : disabledColor;
        }
        
        // 更新按钮的视觉状态
        var buttonImage = rewardLinkButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = allCompleted ? normalColor : disabledColor;
        }
    }

    public void OnRewardButtonClicked()
    {
        if (string.IsNullOrEmpty(rewardURL))
        {
            Debug.LogError("Reward URL is not set!");
            return;
        }

        if (LevelCompletionManager.Instance.AreAllLevelsCompleted())
        {
            OpenURL();
        }
    }

    private void OpenURL()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            // 使用WebGL特定的URL打开方法
            WebGLURLOpener.OpenURL(rewardURL);
        #else
            // 在其他平台使用标准方法
            Application.OpenURL(rewardURL);
        #endif
    }

    public void ResetProgress()
    {
        LevelCompletionManager.Instance.ResetAllProgress();
        UpdateLevelStatus();
        UpdateRewardButton();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (totalLevels <= 0)
        {
            Debug.LogError("Total levels must be greater than 0!");
            totalLevels = 1;
        }
    }
#endif
}