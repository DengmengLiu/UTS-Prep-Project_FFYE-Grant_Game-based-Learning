using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // 绑定全屏黑幕的 Image 组件
    public float fadeDuration = 1f; // 淡入淡出时长

    public void FadeOutAndSwitchCamera(System.Action onComplete)
    {
        StartCoroutine(FadeOut(onComplete));
    }

    private IEnumerator FadeOut(System.Action onComplete)
    {
        fadeImage.gameObject.SetActive(true); // 显示黑幕
        Color color = fadeImage.color;
        
        // 淡入
        while (color.a < 1)
        {
            color.a += Time.deltaTime / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        // 在黑幕完全覆盖后调用回调
        onComplete?.Invoke();

        // 淡出
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // 隐藏黑幕
    }
}
