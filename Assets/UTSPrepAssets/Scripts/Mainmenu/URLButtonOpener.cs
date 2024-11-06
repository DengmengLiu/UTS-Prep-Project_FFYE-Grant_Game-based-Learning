using UnityEngine;
using UnityEngine.UI;

public class URLButtonOpener : MonoBehaviour
{
    [SerializeField] private string url = "https://example.com";
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OpenURL);
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }
    }

    private void OpenURL()
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("URL is not set!");
            return;
        }

        #if UNITY_WEBGL && !UNITY_EDITOR
            WebGLURLOpener.OpenURL(url);
        #else
            Application.OpenURL(url);
        #endif
    }

    public void SetURL(string newURL)
    {
        url = newURL;
    }

    public void SetInteractable(bool isEnabled)
    {
        if (button != null)
        {
            button.interactable = isEnabled;
        }
    }
}
