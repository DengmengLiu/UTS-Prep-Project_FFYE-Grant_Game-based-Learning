using UnityEngine;

public class ObjectFloating : MonoBehaviour 
{
    [Header("Animation Settings")]
    [SerializeField] private bool enableRotation = true;
    [SerializeField] private bool enableBounce = true;
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    
    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.3f;
    [SerializeField] private float bounceSpeed = 2f;

    private Vector3 startPosition;
    private float timeOffset;

    private void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        if (enableRotation)
        {
            // 旋转动画
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }

        if (enableBounce)
        {
            // 弹跳动画
            float newY = startPosition.y + Mathf.Sin((Time.time + timeOffset) * bounceSpeed) * bounceHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    // 提供公共方法来控制动画
    public void SetRotationEnabled(bool enabled)
    {
        enableRotation = enabled;
    }

    public void SetBounceEnabled(bool enabled)
    {
        enableBounce = enabled;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}