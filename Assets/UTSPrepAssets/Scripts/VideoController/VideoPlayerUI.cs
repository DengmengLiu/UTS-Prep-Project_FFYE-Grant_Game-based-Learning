using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(VideoPlayerController))]
public class VideoPlayerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private Button playPauseButton;
    [SerializeField] private Image playIcon;
    [SerializeField] private Image pauseIcon;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button muteButton;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image unmuteIcon;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI totalTimeText;

    [Header("Auto Hide Settings")]
    [SerializeField] private float autoHideDelay = 3f;
    [SerializeField] private bool autoHideControls = true;

    private VideoPlayerController videoPlayer;
    private bool isDraggingProgress = false;
    private float lastInteractionTime;
    private float previousVolume = 1f;
    private bool isMuted = false;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayerController>();
        InitializeUI();
    }

    private void Start()
    {
        // 初始化音量
        volumeSlider.value = 1f;
        UpdateVolumeUI(1f);
    }

    private void Update()
    {
        if (!isDraggingProgress)
        {
            UpdateProgressUI();
        }

        if (autoHideControls)
        {
            CheckAutoHide();
        }
    }

    private void InitializeUI()
    {
        // 设置按钮点击事件
        if (playPauseButton != null)
        {
            playPauseButton.onClick.AddListener(OnPlayPauseClicked);
        }

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(OnMuteClicked);
        }

        // 设置进度条事件
        if (progressSlider != null)
        {
            progressSlider.onValueChanged.AddListener(OnProgressValueChanged);

            // 添加拖动开始和结束事件
            EventTrigger trigger = progressSlider.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
            beginDragEntry.eventID = EventTriggerType.BeginDrag;
            beginDragEntry.callback.AddListener((data) => { OnProgressDragStart((PointerEventData)data); });
            trigger.triggers.Add(beginDragEntry);

            EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
            endDragEntry.eventID = EventTriggerType.EndDrag;
            endDragEntry.callback.AddListener((data) => { OnProgressDragEnd((PointerEventData)data); });
            trigger.triggers.Add(endDragEntry);
        }

        // 设置音量滑块事件
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeValueChanged);
        }

        // 注册视频播放器事件
        if (videoPlayer != null)
        {
            videoPlayer.onVideoStarted.AddListener(OnVideoStarted);
            videoPlayer.onVideoFinished.AddListener(OnVideoFinished);
            videoPlayer.onError.AddListener(OnVideoError);
        }

        // 初始显示暂停图标
        UpdatePlayPauseUI(false);
    }

    private void OnProgressDragStart(PointerEventData data)
    {
        isDraggingProgress = true;
    }

    private void OnProgressDragEnd(PointerEventData data)
    {
        isDraggingProgress = false;
        float targetTime = progressSlider.value;
        videoPlayer.SetTime(targetTime);
    }

    private void OnProgressValueChanged(float value)
    {
        if (isDraggingProgress)
        {
            UpdateTimeText(value * videoPlayer.GetDuration(), videoPlayer.GetDuration());
        }
    }

    private void OnVolumeValueChanged(float value)
    {
        previousVolume = value;
        videoPlayer.SetVolume(value);
        UpdateVolumeUI(value);
    }

    private void OnPlayPauseClicked()
    {
        videoPlayer.PlayPause();
        UpdatePlayPauseUI(videoPlayer.IsPlaying);
        ShowControls(); // 重置自动隐藏计时器
    }

    private void OnMuteClicked()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            previousVolume = volumeSlider.value;
            volumeSlider.value = 0f;
        }
        else
        {
            volumeSlider.value = previousVolume;
        }

        videoPlayer.SetVolume(volumeSlider.value);
        UpdateVolumeUI(volumeSlider.value);
        ShowControls();
    }

    private void UpdateProgressUI()
    {
        if (videoPlayer == null) return;

        float currentTime = videoPlayer.GetCurrentTime();
        float duration = videoPlayer.GetDuration();

        if (duration > 0)
        {
            if (!isDraggingProgress)
            {
                progressSlider.value = currentTime / duration;
            }
            UpdateTimeText(currentTime, duration);
        }
    }

    private void UpdateTimeText(float currentTime, float totalTime)
    {
        if (currentTimeText != null)
        {
            currentTimeText.text = FormatTime(currentTime);
        }

        if (totalTimeText != null && totalTime > 0)
        {
            totalTimeText.text = FormatTime(totalTime);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}", (int)time.TotalMinutes, time.Seconds);
    }

    private void UpdatePlayPauseUI(bool isPlaying)
    {
        if (playIcon != null) playIcon.gameObject.SetActive(!isPlaying);
        if (pauseIcon != null) pauseIcon.gameObject.SetActive(isPlaying);
    }

    private void UpdateVolumeUI(float volume)
    {
        if (muteIcon != null && unmuteIcon != null)
        {
            bool isMuted = volume <= 0f;
            muteIcon.gameObject.SetActive(isMuted);
            unmuteIcon.gameObject.SetActive(!isMuted);
        }
    }

    public void ShowControls()
    {
        if (controlPanel != null)
        {
            controlPanel.SetActive(true);
            lastInteractionTime = Time.time;
        }
    }

    public void HideControls()
    {
        if (controlPanel != null)
        {
            controlPanel.SetActive(false);
        }
    }

    private void CheckAutoHide()
    {
        if (!autoHideControls) return;

        // 检测鼠标是否在控制面板上
        bool isMouseOverUI = EventSystem.current.IsPointerOverGameObject();

        if (isMouseOverUI || isDraggingProgress)
        {
            lastInteractionTime = Time.time;
            ShowControls();
        }
        else if (Time.time - lastInteractionTime > autoHideDelay)
        {
            HideControls();
        }
    }

    private void OnVideoStarted()
    {
        ShowControls();
        UpdatePlayPauseUI(true);
    }

    private void OnVideoFinished()
    {
        UpdatePlayPauseUI(false);
        ShowControls();
    }

    private void OnVideoError(string error)
    {
        Debug.LogError($"Video Error: {error}");
        // 这里可以添加错误UI显示
    }

    private void OnDestroy()
    {
        // 清理事件监听
        if (videoPlayer != null)
        {
            videoPlayer.onVideoStarted.RemoveListener(OnVideoStarted);
            videoPlayer.onVideoFinished.RemoveListener(OnVideoFinished);
            videoPlayer.onError.RemoveListener(OnVideoError);
        }
    }
}