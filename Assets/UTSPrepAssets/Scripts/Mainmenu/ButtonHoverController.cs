using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 两个 TextMeshPro 组件，分别在 hover 时启用/禁用
    public TextMeshProUGUI textMeshProHover; // 在 hover 时启用的 TextMeshPro
    public TextMeshProUGUI textMeshProNormal; // 在非 hover 时启用的 TextMeshPro

    void Start()
    {
        // 确保两个 TextMeshPro 组件不为空
        if (textMeshProHover == null || textMeshProNormal == null)
        {
            Debug.LogError("请在 Inspector 中分配两个 TextMeshPro 组件。");
        }

        // 初始状态：禁用 hover 组件，启用 normal 组件
        textMeshProHover.gameObject.SetActive(false);
        textMeshProNormal.gameObject.SetActive(true);
    }

    // 当鼠标进入按钮时，启用第一个 TextMeshPro，禁用第二个
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshProHover != null && textMeshProNormal != null)
        {
            textMeshProHover.gameObject.SetActive(true); // 启用 hover 文本
            textMeshProNormal.gameObject.SetActive(false); // 禁用 normal 文本
        }
    }

    // 当鼠标离开按钮时，禁用第一个 TextMeshPro，启用第二个
    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshProHover != null && textMeshProNormal != null)
        {
            textMeshProHover.gameObject.SetActive(false); // 禁用 hover 文本
            textMeshProNormal.gameObject.SetActive(true); // 启用 normal 文本
        }
    }
}
