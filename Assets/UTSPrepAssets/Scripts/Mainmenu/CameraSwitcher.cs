using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public CinemachineVirtualCamera targetCamera;
    
    public FadeController fadeController;
    public bool useFade;
    
    public void SwitchCam(int priority)
    {
        if (useFade)
        {
            fadeController.FadeOutAndSwitchCamera(() =>
            {
                SwitchToTargetCamera(priority);
            });
        }
        else
        {
            SwitchToTargetCamera(priority);
        }
    }
    private void SwitchToTargetCamera(int priority)
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 10;
        }

        if (targetCamera != null)
        {
            targetCamera.Priority = priority;
        }
    }
}
