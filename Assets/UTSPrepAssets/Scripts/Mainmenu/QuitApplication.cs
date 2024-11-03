using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    // 当按钮点击时调用此方法
    public void QuitGame()
    {
        // 在编辑器中调试时，退出不会生效，所以用 Debug 提示
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 在实际的构建版本中，退出游戏
        Application.Quit();
        #endif
        
        Debug.Log("Game is exiting...");
    }
}
