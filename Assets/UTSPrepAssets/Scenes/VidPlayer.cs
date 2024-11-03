using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{

    [SerializeField] string videoFileName;
    // Start is called before the first frame update

    private void OnEnable()
    {
        // Your function call here
        PlayVideo();
    }
    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        // 在 WebGL 中，`Application.streamingAssetsPath` 会返回一个 URL
        string videoPath = Application.streamingAssetsPath + "/" + videoFileName;
        Debug.Log("Video path: " + videoPath);

        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }

}