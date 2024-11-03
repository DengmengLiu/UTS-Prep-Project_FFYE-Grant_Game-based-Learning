// InteractableObject.cs - 可交互物体的示例实现
using UnityEngine.Events;
using UnityEngine;
using DialogueEditor;

public class BookPileInteractive : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "Take Quiz";
    [SerializeField] private UnityEvent onInteractionStart;
    [SerializeField] private UnityEvent onInteractionEnd;

    public void OnInteractionStart()
    {
        onInteractionStart?.Invoke();
    }

    public void OnInteractionEnd()
    {
        onInteractionEnd?.Invoke();
    }

    public string GetInteractionPrompt()
    {
        return promptText;
    }
}