// CharacterAnimation.cs - 动画控制器
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Range(0, 1f)]
    public float startAnimationTransitionTime = 0.1f;
    [Range(0, 1f)]
    public float stopAnimationTransitionTime = 0.15f;

    private Animator animator;
    private static readonly int MovementSpeedParam = Animator.StringToHash("Blend");
    private float currentAnimationSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateMovementAnimation(float normalizedSpeed)
    {
        if (animator == null) return;

        // 根据是开始移动还是停止移动选择不同的过渡时间
        float transitionTime = normalizedSpeed > currentAnimationSpeed 
            ? startAnimationTransitionTime 
            : stopAnimationTransitionTime;

        // 更新动画混合参数
        animator.SetFloat(MovementSpeedParam, normalizedSpeed, transitionTime, Time.deltaTime);
        currentAnimationSpeed = normalizedSpeed;
    }

    public void ResetAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat(MovementSpeedParam, 0f);
            currentAnimationSpeed = 0f;
        }
    }
}