using UnityEngine;
using UnityEngine.Events;

public class TelevisionInteractive : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "Talk";

    [Header("Television State")]
    [SerializeField] private Material screenOnMaterial;     // 电视开启时的材质
    [SerializeField] private Material screenOffMaterial;    // 电视关闭时的材质
    [SerializeField] private Renderer screenRenderer;       // 电视屏幕的渲染器

    [Header("Events")]
    public UnityEvent onTelevisionTurnOn;    // 电视开启事件
    public UnityEvent onTelevisionTurnOff;   // 电视关闭事件

    private bool isOn = false;               // 电视是否开启

    private void Start()
    {
        // 确保组件引用正确
        if (screenRenderer == null)
        {
            screenRenderer = GetComponent<Renderer>();
        }

        // 初始化电视状态
        UpdateTelevisionState(false);
    }

    // 实现IInteractable接口
    public void OnInteractionStart()
    {
        ToggleTelevision();
    }

    public void OnInteractionEnd()
    {
        if (isOn)
        {
            // 如果电视是开着的，关闭它
            UpdateTelevisionState(false);
        }
    }

    public string GetInteractionPrompt()
    {
        return promptText;
    }

    // 切换电视状态
    private void ToggleTelevision()
    {
        UpdateTelevisionState(!isOn);
    }

    // 更新电视状态
    private void UpdateTelevisionState(bool turnOn)
    {
        isOn = turnOn;

        // 更新材质
        if (screenRenderer != null && screenOnMaterial != null && screenOffMaterial != null)
        {
            // 获取当前的材质数组
            Material[] materials = screenRenderer.materials;

            // 确保数组有足够的元素以替换第二个材质
            if (materials.Length > 1)
            {
                // 根据条件切换第二个材质
                materials[1] = isOn ? screenOnMaterial : screenOffMaterial;

                // 将修改后的材质数组赋回
                screenRenderer.materials = materials;
            }
        }

        // 触发相应事件
        if (isOn)
        {
            onTelevisionTurnOn?.Invoke();
            // 这里之后会调用视频管理器来播放视频
            PlayVideo();
        }
        else
        {
            onTelevisionTurnOff?.Invoke();
            // 这里之后会调用视频管理器来停止视频
            StopVideo();
        }
    }

    // 播放视频
    private void PlayVideo()
    {
        
    }

    // 停止视频
    private void StopVideo()
    {
        Debug.Log("Stopping video");
    }

    // 设置视频ID
    public void SetVideoId(string newVideoId)
    {
        if (string.IsNullOrEmpty(newVideoId))
        {
            Debug.LogError("Invalid video ID");
            return;
        }

        if (isOn)
        {
            // 如果电视是开着的，重新播放新的视频
            PlayVideo();
        }
    }

    // 直接控制电视开关
    public void TurnOn()
    {
        if (!isOn)
        {
            UpdateTelevisionState(true);
        }
    }

    public void TurnOff()
    {
        if (isOn)
        {
            UpdateTelevisionState(false);
        }
    }
}