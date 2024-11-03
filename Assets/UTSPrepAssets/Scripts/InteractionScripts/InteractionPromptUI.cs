// InteractionPromptUI.cs - 处理UI提示显示
using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Color color;

    public void ShowPrompt(string text)
    {
        if (promptText != null)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(color);
            promptText.text = $"Press<#{colorHex}><size=125%>  <sprite=42> </size></color>to {text}";
        }
    }

    public void HidePrompt()
    {
        if (promptText != null)
        {
            promptText.text = "";
        }
    }
}