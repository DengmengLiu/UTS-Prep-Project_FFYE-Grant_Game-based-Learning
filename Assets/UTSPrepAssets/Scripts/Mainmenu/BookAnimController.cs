using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookAnimController : MonoBehaviour
{
    private Animator animator; // 需要控制的 Animator
    public UnityEvent onOPENAnimationComplete;
    public UnityEvent onCLOSEAnimationComplete;
    private bool isOpen = false;
    public float delayBeforeInvoke = 2f;
    void Awake() 
    {
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        // 在游戏开始时禁用 Animator
        if (animator != null)
        {
            animator.enabled = false;
        }
        else
        {
            Debug.LogWarning("Animator component not found on this GameObject.");
        }
    }

    public void EnableAnim()
    {
        animator.enabled = true;      
        StartCoroutine(WaitForAnimationComplete());
    }

    private IEnumerator WaitForAnimationComplete()
    {
        
        isOpen = !isOpen; 
        animator.SetBool("IsOpen", isOpen); 

        
        if (isOpen)
        {   
            yield return new WaitForSeconds(delayBeforeInvoke); 
            onOPENAnimationComplete.Invoke(); 
        }
        else
        {         
            yield return new WaitForSeconds(delayBeforeInvoke); 
            onCLOSEAnimationComplete.Invoke(); 
        }
    }
}
