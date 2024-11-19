using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool isCursorLocked = false;

    private void Start()
    {
        // 设置初始状态
        SetCursorState(isCursorLocked);
    }

    public void ToggleCursorState()
    {
        // 切换状态
        isCursorLocked = !isCursorLocked;
        SetCursorState(isCursorLocked);
    }

    private void SetCursorState(bool locked)
    {
        // 设置鼠标是否可见
        Cursor.visible = !locked;
        
        // 设置鼠标锁定状态
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        
        Debug.Log($"Cursor is now {(locked ? "locked" : "unlocked")} and {(locked ? "hidden" : "visible")}");
    }

    // 可选：添加快捷键控制
    private void Update()
    {
        // 示例：按 Alt 键切换鼠标状态
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ToggleCursorState();
        }
    }
}