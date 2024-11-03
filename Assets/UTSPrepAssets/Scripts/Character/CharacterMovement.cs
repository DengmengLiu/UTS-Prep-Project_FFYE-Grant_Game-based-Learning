// CharacterMovement.cs
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = 9.81f;

    [Header("Animation Smoothing")]
    public float movementSmoothTime = 0.1f;
    
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool canMove = true;
    private float verticalVelocity;
    private CharacterAnimation characterAnimation;
    private float currentSpeed;
    private float speedVelocity;
    private Camera mainCamera;

     private void Awake()
    {
        // 在游戏开始时立即锁定并隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        characterAnimation = GetComponent<CharacterAnimation>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!canMove) 
        {
            UpdateAnimation(0);
            return;
        }

        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        // 获取输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontalInput, 0, verticalInput);
        float targetSpeed = input.magnitude;

        // 平滑处理速度变化
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, movementSmoothTime);

        if (input.magnitude > 0.1f)
        {
            // 获取相机前方和右方向（忽略Y轴）
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // 根据相机方向计算移动方向
            moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

            // 应用移动
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // 平滑旋转角色面向移动方向
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // 更新动画
        UpdateAnimation(currentSpeed);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
        controller.Move(verticalMovement * Time.deltaTime);
    }

    private void UpdateAnimation(float speed)
    {
        if (characterAnimation != null)
        {
            characterAnimation.UpdateMovementAnimation(speed);
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        if (!enabled)
        {
            currentSpeed = 0;
            UpdateAnimation(0);
        }
    }
}