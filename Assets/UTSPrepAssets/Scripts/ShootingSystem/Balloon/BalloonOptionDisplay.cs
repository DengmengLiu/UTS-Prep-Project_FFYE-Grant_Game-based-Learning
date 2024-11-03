using UnityEngine;
using TMPro;

public class BalloonOptionDisplay : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshPro optionText;  // 直接引用TextMeshPro组件
    [SerializeField] private Vector3 textOffset = new Vector3(0, 0, 0.5f);
    [SerializeField] private float textSize = 1f;

    private Transform cameraTransform;

    private void Awake()
    {
        // 如果没有手动指定，尝试获取或创建TextMeshPro组件
        if (optionText == null)
        {
            optionText = GetComponentInChildren<TextMeshPro>();
            if (optionText == null)
            {
                CreateTextDisplay();
            }
        }

        if (optionText != null)
        {
            optionText.transform.localPosition = textOffset;
            optionText.fontSize = textSize;
            optionText.alignment = TextAlignmentOptions.Center;
        }
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void CreateTextDisplay()
    {
        GameObject textObj = new GameObject("OptionText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = textOffset;
        optionText = textObj.AddComponent<TextMeshPro>();
        optionText.fontSize = textSize;
        optionText.alignment = TextAlignmentOptions.Center;
        optionText.text = ""; // 清空默认文本
    }

    private void LateUpdate()
    {
        // 让文本始终面向摄像机
        if (optionText != null && cameraTransform != null)
        {
            optionText.transform.LookAt(cameraTransform);
            optionText.transform.Rotate(0, 180, 0);
        }
    }

    public void SetOptionText(string text)
    {
        if (optionText != null)
        {
            optionText.text = text;
            //Debug.Log($"Setting balloon option text to: {text}"); // 添加调试日志
        }
        else
        {
            Debug.LogError("TextMeshPro component not found on balloon!");
        }
    }
}