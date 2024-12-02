using UnityEngine;
using UnityEngine.UI; // 用于 Button
using TMPro; // 用于 TextMeshPro

public class ReminderController : MonoBehaviour
{
    public GameObject panel; // 关联 Panel 对象
    public Button okButton; // 关联 OK 按钮
    public TextMeshProUGUI panelText; // 关联 Panel 下的 TextMeshProUGUI
    private const string PanelStateKey = "PanelState"; // 用于保存状态的键值

    void Start()
    {
        // 每次场景加载时检查
        if (LevelCompletionManager.Instance.AreAllLevelsCompleted())
        {
            // 如果关卡完成，显示 Panel 并更新文本
            ShowPanel("Congratulations! All levels are completed.\nPlease go to <color=#FF8E8E>Claim Rewards Level.");
        }
        else
        {
            // 检查是否需要隐藏 Panel（如果之前点击了 OK 按钮）
            if (PlayerPrefs.HasKey(PanelStateKey) && PlayerPrefs.GetInt(PanelStateKey) == 0)
            {
                panel.SetActive(false);
            }
            else
            {
                ShowPanel("Welcome! Complete <color=#FF8E8E>Level1 and Level2</color> to unlock rewards.");
            }
        }

        // 为 OK 按钮添加监听事件
        if (okButton != null)
        {
            okButton.onClick.AddListener(HidePanelOnOk);
        }
    }

    // 显示 Panel 并更新文本内容
    private void ShowPanel(string message)
    {
        if (panel != null)
        {
            panel.SetActive(true); // 启用 Panel
            if (panelText != null)
            {
                panelText.text = message; // 更新文本内容
            }
        }
    }

    // 隐藏 Panel 并保存状态
    private void HidePanelOnOk()
    {
        if (panel != null)
        {
            panel.SetActive(false); // 隐藏 Panel
        }
        PlayerPrefs.SetInt(PanelStateKey, 0); // 保存状态，表示用户已点击 OK
        PlayerPrefs.Save(); // 确保状态被保存
    }
}
