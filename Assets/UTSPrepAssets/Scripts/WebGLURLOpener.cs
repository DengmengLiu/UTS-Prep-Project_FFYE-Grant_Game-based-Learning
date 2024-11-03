using UnityEngine;
using System.Runtime.InteropServices;

public class WebGLURLOpener : MonoBehaviour 
{
    // 在WebGL中调用JavaScript函数
    [DllImport("__Internal")]
    private static extern void OpenURLInNewTab(string url);

    public static void OpenURL(string url)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            OpenURLInNewTab(url);
        #else
            Application.OpenURL(url);
        #endif
    }
}