// InteractionDetector.cs - 添加强制检查方法
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private float detectionDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayer;

    private IInteractionEvents events;

    private void Awake()
    {
        events = GetComponent<IInteractionEvents>();
    }

    private void Update()
    {
        PerformDetection();
    }

    private void PerformDetection()
    {
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                events?.OnDetectedInteractable(interactable, hit.collider.gameObject);
                return;
            }
        }

        events?.OnLostInteractable();
    }

    // 添加强制检查方法
    public void ForceCheck()
    {
        PerformDetection();
    }

    private void OnDrawGizmos()
    {
        if (raycastOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(raycastOrigin.position, raycastOrigin.forward * detectionDistance);
        }
    }
}