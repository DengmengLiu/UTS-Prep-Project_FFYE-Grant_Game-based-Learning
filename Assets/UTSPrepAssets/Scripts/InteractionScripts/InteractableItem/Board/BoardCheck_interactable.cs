using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardCheck_interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "Check";
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
