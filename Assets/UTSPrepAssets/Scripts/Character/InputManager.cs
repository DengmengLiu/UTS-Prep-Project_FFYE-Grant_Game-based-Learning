// InputManager.cs - 输入管理
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void InputEventHandler(Vector2 moveInput);
    public event InputEventHandler OnMovementInput;
    
    public delegate void CameraToggleHandler();
    public event CameraToggleHandler OnCameraToggle;

    private void Update()
    {
        HandleMovementInput();
        HandleCameraToggle();
    }

    private void HandleMovementInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnMovementInput?.Invoke(input);
    }

    private void HandleCameraToggle()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnCameraToggle?.Invoke();
        }
    }
}