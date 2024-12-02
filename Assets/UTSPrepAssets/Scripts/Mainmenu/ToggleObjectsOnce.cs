using UnityEngine;

public class ToggleObjectsOnce : MonoBehaviour
{
    [SerializeField] private GameObject objectA;
    [SerializeField] private GameObject objectB;

    private int callCount = 0;

    public void ToggleObjects()
    {
        if (callCount == 0)
        {
            if (objectA != null && objectB != null)
            {
                objectA.SetActive(false);
                objectB.SetActive(true);
                callCount++;
            }
        }
        else if (callCount == 1)
        {
            if (objectB != null)
            {
                objectB.SetActive(false);
                callCount++;
            }
        }
    }
}
