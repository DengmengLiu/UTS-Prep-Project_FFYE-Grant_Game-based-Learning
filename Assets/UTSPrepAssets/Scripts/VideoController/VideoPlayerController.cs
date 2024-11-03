using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerController : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField] private string videoFileName;
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private bool loopVideo = false;

    [Header("Events")]
    public UnityEvent onVideoStarted;
    public UnityEvent onVideoFinished;
    public UnityEvent<string> onError;

    private VideoPlayer videoPlayer;
    private bool isInitialized = false;
    private float currentVolume = 1f;
    private bool isPaused = true;

    // 公开属性
    public bool IsPlaying => videoPlayer != null && videoPlayer.isPlaying;
    public bool IsPrepared => videoPlayer != null && videoPlayer.isPrepared;
    public bool IsLooping => videoPlayer != null && videoPlayer.isLooping;

    private void Awake()
    {
        InitializeVideoPlayer();
    }

    private void OnEnable()
    {
        if (playOnEnable && !string.IsNullOrEmpty(videoFileName))
        {
            LoadAndPlayVideo();
        }
    }

    private void InitializeVideoPlayer()
    {
        if (isInitialized) return;

        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            // 配置视频播放器
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = loopVideo;
            videoPlayer.source = VideoSource.Url;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;

            // 注册事件处理
            videoPlayer.errorReceived += HandleVideoError;
            videoPlayer.started += HandleVideoStarted;
            videoPlayer.loopPointReached += HandleVideoFinished;
            videoPlayer.prepareCompleted += HandlePrepareCompleted;

            isInitialized = true;

            // 设置初始音量
            SetVolume(currentVolume);
        }
        else
        {
            Debug.LogError("VideoPlayer component not found!");
        }
    }

    public void LoadAndPlayVideo(string fileName = null)
    {
        if (!isInitialized)
        {
            InitializeVideoPlayer();
        }

        if (fileName != null)
        {
            videoFileName = fileName;
        }

        if (string.IsNullOrEmpty(videoFileName))
        {
            HandleVideoError(null, "No video file specified");
            return;
        }

        try
        {
            // 构建视频URL
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

#if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL特殊处理
            if (!videoPath.StartsWith("http"))
            {
                videoPath = $"{Application.absoluteURL.Substring(0, Application.absoluteURL.LastIndexOf('/'))}/StreamingAssets/{videoFileName}";
            }
#endif

            Debug.Log($"Loading video from: {videoPath}");

            videoPlayer.url = videoPath;
            videoPlayer.Prepare();
            isPaused = false;
        }
        catch (Exception ex)
        {
            HandleVideoError(null, $"Failed to load video: {ex.Message}");
        }
    }

    private void HandlePrepareCompleted(VideoPlayer source)
    {
        if (!isPaused)
        {
            source.Play();
        }
    }

    public void PlayPause()
    {
        if (!IsPrepared)
        {
            LoadAndPlayVideo();
            return;
        }

        if (IsPlaying)
        {
            videoPlayer.Pause();
            isPaused = true;
        }
        else
        {
            videoPlayer.Play();
            isPaused = false;
        }
    }

    public void Stop()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            isPaused = true;
        }
    }

    public void SetTime(float progressPercent)
    {
        if (videoPlayer != null && videoPlayer.frameCount > 0)
        {
            // 计算目标时间
            float targetTime = (progressPercent * (float)videoPlayer.length);

            // 设置视频时间
            videoPlayer.time = targetTime;

            // 如果视频暂停了，确保画面更新
            if (!IsPlaying)
            {
                videoPlayer.StepForward();
            }
        }
    }

    public void SetVolume(float volume)
    {
        currentVolume = Mathf.Clamp01(volume);
        if (videoPlayer != null)
        {
            // 如果使用AudioSource输出
            AudioSource audioSource = videoPlayer.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = currentVolume;
            }
            else
            {
                // 直接设置视频播放器的音量
                videoPlayer.SetDirectAudioVolume(0, currentVolume);
            }
        }
    }

    public float GetCurrentTime()
    {
        return videoPlayer != null ? (float)videoPlayer.time : 0f;
    }

    public float GetDuration()
    {
        return videoPlayer != null ? (float)videoPlayer.length : 0f;
    }

    private void HandleVideoStarted(VideoPlayer source)
    {
        isPaused = false;
        onVideoStarted?.Invoke();
    }

    private void HandleVideoFinished(VideoPlayer source)
    {
        isPaused = true;
        onVideoFinished?.Invoke();
    }

    private void HandleVideoError(VideoPlayer source, string message)
    {
        Debug.LogError($"Video Player Error: {message}");
        onError?.Invoke(message);
    }

    private void OnDisable()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.errorReceived -= HandleVideoError;
            videoPlayer.started -= HandleVideoStarted;
            videoPlayer.loopPointReached -= HandleVideoFinished;
            videoPlayer.prepareCompleted -= HandlePrepareCompleted;
        }
    }

    // 调试方法
    private void OnValidate()
    {
        if (videoPlayer != null)
        {
            videoPlayer.isLooping = loopVideo;
        }
    }
}