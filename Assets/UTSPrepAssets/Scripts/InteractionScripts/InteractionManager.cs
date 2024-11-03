using UnityEngine;

public class InteractionManager : MonoBehaviour, IInteractionEvents
{
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private InteractionDetector detector; // 添加检测器引用

    private bool isInteracting;
    private IInteractable currentInteractable;
    private GameObject currentInteractableObject;

    private void Update()
    {
        // 只在非交互状态下检查输入
        if (!isInteracting && currentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartInteraction();
            }
        }
    }

    public void OnDetectedInteractable(IInteractable interactable, GameObject gameObject)
    {
        if (currentInteractable != interactable)
        {
            currentInteractable = interactable;
            currentInteractableObject = gameObject;

            if (!isInteracting)
            {
                promptUI.ShowPrompt(interactable.GetInteractionPrompt());
            }
        }
    }

    public void OnLostInteractable()
    {
        if (currentInteractable != null)
        {
            // 如果正在交互中失去了目标，结束交互
            if (isInteracting)
            {
                EndInteraction();
            }

            currentInteractable = null;
            currentInteractableObject = null;
            promptUI.HidePrompt();
        }
    }

    private void StartInteraction()
    {
        if (currentInteractable != null && !isInteracting)
        {
            isInteracting = true;
            promptUI.HidePrompt();
            currentInteractable.OnInteractionStart();
        }
    }

    public void EndInteraction()
    {
        if (isInteracting)
        {
            // 保存临时引用
            var interactableToEnd = currentInteractable;

            // 重置状态
            isInteracting = false;

            // 如果还有引用，调用结束回调
            if (interactableToEnd != null)
            {
                interactableToEnd.OnInteractionEnd();
            }

            // 重新检查当前状态
            detector.ForceCheck();
        }
    }
}