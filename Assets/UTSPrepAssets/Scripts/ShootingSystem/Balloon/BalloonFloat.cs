using UnityEngine;

public class BalloonFloat : MonoBehaviour
{
    [System.Serializable]
    public class FloatRange
    {
        public float min;
        public float max;

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetRandom()
        {
            return Random.Range(min, max);
        }
    }

    [Header("Vertical Float Settings")]
    [Tooltip("漂浮幅度范围")]
    [SerializeField] private FloatRange floatAmplitudeRange = new FloatRange(0.3f, 0.7f);
    
    [Tooltip("漂浮速度范围")]
    [SerializeField] private FloatRange floatSpeedRange = new FloatRange(0.8f, 1.2f);

    [Header("Horizontal Sway Settings")]
    [Tooltip("是否启用水平摆动")]
    [SerializeField] private bool enableSway = true;
    
    [Tooltip("摆动幅度范围")]
    [SerializeField] private FloatRange swayAmplitudeRange = new FloatRange(0.2f, 0.4f);
    
    [Tooltip("摆动速度范围")]
    [SerializeField] private FloatRange swaySpeedRange = new FloatRange(0.4f, 0.6f);

    // 每个气球的实际运动参数
    private float floatAmplitude;
    private float floatSpeed;
    private float swayAmplitude;
    private float swaySpeed;
    private Vector3 startPosition;
    private float timeOffset;

    private void Start()
    {
        InitializeRandomParameters();
    }

    public void InitializeRandomParameters()
    {
        // 为每个气球随机设置运动参数
        floatAmplitude = floatAmplitudeRange.GetRandom();
        floatSpeed = floatSpeedRange.GetRandom();
        swayAmplitude = swayAmplitudeRange.GetRandom();
        swaySpeed = swaySpeedRange.GetRandom();
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // 随机初始相位
        
        startPosition = transform.position;
    }

    private void Update()
    {
        // 垂直漂浮
        float verticalOffset = Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatAmplitude;
        
        // 水平摆动
        float horizontalOffset = 0f;
        if (enableSway)
        {
            horizontalOffset = Mathf.Sin((Time.time + timeOffset) * swaySpeed) * swayAmplitude;
        }

        // 应用运动
        Vector3 newPosition = startPosition;
        newPosition.y += verticalOffset;
        newPosition.x += horizontalOffset;
        
        transform.position = newPosition;
    }

    public void ResetPosition(Vector3 position)
    {
        startPosition = position;
        InitializeRandomParameters(); // 重置时重新随机参数
        transform.position = position;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 确保范围值的有效性
        floatAmplitudeRange.min = Mathf.Max(0, floatAmplitudeRange.min);
        floatAmplitudeRange.max = Mathf.Max(floatAmplitudeRange.min, floatAmplitudeRange.max);
        
        floatSpeedRange.min = Mathf.Max(0, floatSpeedRange.min);
        floatSpeedRange.max = Mathf.Max(floatSpeedRange.min, floatSpeedRange.max);
        
        swayAmplitudeRange.min = Mathf.Max(0, swayAmplitudeRange.min);
        swayAmplitudeRange.max = Mathf.Max(swayAmplitudeRange.min, swayAmplitudeRange.max);
        
        swaySpeedRange.min = Mathf.Max(0, swaySpeedRange.min);
        swaySpeedRange.max = Mathf.Max(swaySpeedRange.min, swaySpeedRange.max);
    }
#endif
}