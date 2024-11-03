using UnityEngine;
using DialogueEditor;

public class InputOptionSelectMethod : MonoBehaviour
{
    private void Update()
    {
        if(ConversationManager.Instance!= null)
        {
            if (ConversationManager.Instance.IsConversationActive)
            {
                if(Input.GetKeyDown(KeyCode.W))
                ConversationManager.Instance.SelectPreviousOption();
                else if (Input.GetKeyDown(KeyCode.S))
                ConversationManager.Instance.SelectNextOption();
                else if (Input.GetKeyDown(KeyCode.E))
                ConversationManager.Instance.PressSelectedOption();
            }
        }
    }
}