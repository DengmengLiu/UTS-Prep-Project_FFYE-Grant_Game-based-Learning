// CameraController.cs
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    [Header("Camera References")]
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;
    public Transform cameraRoot;
    
    [Header("First Person Settings")]
    public float mouseSensitivity = 1f;
    public float maxLookUpAngle = 80f;
    public float maxLookDownAngle = 80f;

    [Header("Events")]
    public UnityEvent onEnterFirstPerson;
    public UnityEvent onExitFirstPerson;

    private bool isFirstPerson;
    private float verticalRotation;
    private float horizontalRotation;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private CharacterMovement characterMovement;

    private void Start()
    {
        SetupCamera();
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void SetupCamera()
    {
        if (cameraRoot == null && firstPersonCam != null)
        {
            cameraRoot = firstPersonCam.transform;
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 默认第三人称视角
        SwitchToThirdPerson();
    }

    private void Update()
    {
        if (isFirstPerson)
        {
            UpdateFirstPersonCamera();
        }
    }

    private void UpdateFirstPersonCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalRotation += mouseX;
        verticalRotation = Mathf.Clamp(verticalRotation - mouseY, -maxLookDownAngle, maxLookUpAngle);

        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        if (cameraRoot != null)
        {
            cameraRoot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }

    public void EnterFirstPersonView()
    {
        if (!isFirstPerson)
        {
            isFirstPerson = true;
            firstPersonCam.Priority = 20;
            thirdPersonCam.Priority = 10;

            // 保存当前位置和旋转
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            // 重置旋转到初始状态
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (cameraRoot != null)
            {
                cameraRoot.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            // 重置视角控制变量
            verticalRotation = 0f;
            horizontalRotation = 0f;

            // 禁用移动
            if (characterMovement != null)
            {
                characterMovement.SetMovementEnabled(false);
            }

            onEnterFirstPerson?.Invoke();
        }
    }

    public void ExitFirstPersonView()
    {
        if (isFirstPerson)
        {
            isFirstPerson = false;
            firstPersonCam.Priority = 10;
            thirdPersonCam.Priority = 20;

            // 恢复原始位置和旋转
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            // 重置相机根节点的旋转
            if (cameraRoot != null)
            {
                cameraRoot.localRotation = Quaternion.identity;
            }

            // 启用移动
            if (characterMovement != null)
            {
                characterMovement.SetMovementEnabled(true);
            }

            onExitFirstPerson?.Invoke();
        }
    }

    private void SwitchToThirdPerson()
    {
        firstPersonCam.Priority = 10;
        thirdPersonCam.Priority = 20;
        isFirstPerson = false;
    }

    private void OnDisable()
    {
        if (isFirstPerson)
        {
            ExitFirstPersonView();
        }
    }
}