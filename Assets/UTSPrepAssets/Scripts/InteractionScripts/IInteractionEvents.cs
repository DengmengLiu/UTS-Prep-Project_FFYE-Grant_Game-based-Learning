// IInteractionEvents.cs - 交互事件接口
using UnityEngine;

public interface IInteractionEvents
{
    void OnDetectedInteractable(IInteractable interactable, GameObject gameObject);
    void OnLostInteractable();
}