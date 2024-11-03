using UnityEngine;
using UnityEngine.Events;

public class WaypointTrigger : MonoBehaviour 
{
    [SerializeField] private string playerTag = "Player";
    public UnityEvent onPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            onPlayerEnter?.Invoke();
        }
    }
}