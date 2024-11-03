// IInteractable.cs - 交互接口
public interface IInteractable
{
    void OnInteractionStart();
    void OnInteractionEnd();
    string GetInteractionPrompt();
}