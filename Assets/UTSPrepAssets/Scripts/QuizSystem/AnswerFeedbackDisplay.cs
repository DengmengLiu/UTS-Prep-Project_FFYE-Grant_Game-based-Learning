using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AnswerFeedbackDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image feedbackIcon;
    [SerializeField] private TextMeshProUGUI feedbackText; // 可选的文本反馈

    [Header("Icons")]
    [SerializeField] private Sprite correctIcon; // 对勾图标
    [SerializeField] private Sprite wrongIcon;   // 叉号图标

    [Header("Animation Settings")]
    [SerializeField] private float showDuration = 1.0f;    // 显示持续时间
    [SerializeField] private float fadeInTime = 0.2f;      // 淡入时间
    [SerializeField] private float fadeOutTime = 0.3f;     // 淡出时间
    [SerializeField] private float scaleMultiplier = 1.2f; // 缩放倍数

    [Header("Colors")]
    [SerializeField] private Color correctColor = new Color(0.2f, 0.8f, 0.2f, 1f); // 绿色
    [SerializeField] private Color wrongColor = new Color(0.8f, 0.2f, 0.2f, 1f);   // 红色

    private void Start()
    {
        // 确保开始时隐藏
        if (feedbackIcon != null)
        {
            feedbackIcon.gameObject.SetActive(false);
        }
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    public void ShowCorrectFeedback()
    {
        ShowFeedback(true);
    }

    public void ShowWrongFeedback()
    {
        ShowFeedback(false);
    }

    private void ShowFeedback(bool isCorrect)
    {
        // 停止所有正在运行的协程
        StopAllCoroutines();

        // 设置正确的图标和颜色
        if (feedbackIcon != null)
        {
            feedbackIcon.sprite = isCorrect ? correctIcon : wrongIcon;
            feedbackIcon.color = isCorrect ? correctColor : wrongColor;
        }

        // 设置文本（如果有）
        if (feedbackText != null)
        {
            feedbackText.text = isCorrect ? "Correct!" : "Wrong!";
            feedbackText.color = isCorrect ? correctColor : wrongColor;
        }

        // 开始动画
        StartCoroutine(AnimateFeedback());
    }

    private IEnumerator AnimateFeedback()
    {
        // 准备动画
        if (feedbackIcon != null)
        {
            feedbackIcon.gameObject.SetActive(true);
            feedbackIcon.transform.localScale = Vector3.zero;
            feedbackIcon.color = new Color(
                feedbackIcon.color.r,
                feedbackIcon.color.g,
                feedbackIcon.color.b,
                0f
            );
        }

        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.alpha = 0f;
        }

        // 淡入并放大
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeInTime;

            // 使用平滑的缓动函数
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // 更新缩放
            if (feedbackIcon != null)
            {
                feedbackIcon.transform.localScale = Vector3.one * smoothT * scaleMultiplier;
                feedbackIcon.color = new Color(
                    feedbackIcon.color.r,
                    feedbackIcon.color.g,
                    feedbackIcon.color.b,
                    smoothT
                );
            }

            if (feedbackText != null)
            {
                feedbackText.alpha = smoothT;
            }

            yield return null;
        }

        // 缩小到正常大小
        elapsedTime = 0f;
        float shrinkTime = 0.1f;
        Vector3 startScale = Vector3.one * scaleMultiplier;
        while (elapsedTime < shrinkTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shrinkTime;
            
            if (feedbackIcon != null)
            {
                feedbackIcon.transform.localScale = Vector3.Lerp(startScale, Vector3.one, t);
            }

            yield return null;
        }

        // 保持显示一段时间
        yield return new WaitForSeconds(showDuration);

        // 淡出
        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeOutTime;
            float alpha = 1f - Mathf.SmoothStep(0f, 1f, t);

            if (feedbackIcon != null)
            {
                feedbackIcon.color = new Color(
                    feedbackIcon.color.r,
                    feedbackIcon.color.g,
                    feedbackIcon.color.b,
                    alpha
                );
            }

            if (feedbackText != null)
            {
                feedbackText.alpha = alpha;
            }

            yield return null;
        }

        // 隐藏物体
        if (feedbackIcon != null)
        {
            feedbackIcon.gameObject.SetActive(false);
        }
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }
}