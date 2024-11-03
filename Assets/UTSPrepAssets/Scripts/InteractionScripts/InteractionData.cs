// InteractionData.cs - 存储交互相关的数据结构
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractionTagInfo
{
    public string tag;
    public string promptText;
    public KeyCode interactionKey = KeyCode.E;
    public Color promptColor = new Color(223f / 255f, 114f / 255f, 128f / 255f);
    public UnityEvent onInteractionStart;
    public UnityEvent onInteractionEnd;
}