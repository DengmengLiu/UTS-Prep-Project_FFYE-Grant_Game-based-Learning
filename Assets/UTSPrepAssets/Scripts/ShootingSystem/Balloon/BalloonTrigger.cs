using UnityEngine;

public class BalloonTrigger : MonoBehaviour
{
    [Header("Balloon Properties")]
    public bool isCorrectBalloon = false;
    public bool isExitBalloon = false;  // 新添加的属性
    
    [Header("References")]
    public QuizManager quizManager;
    private BalloonOptionDisplay optionDisplay;
    private BalloonExplosion balloonExplosion;
    
    [Header("Effects")]
    //[SerializeField] private ParticleSystem correctEffect;
    //[SerializeField] private ParticleSystem wrongEffect;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioSource balloonPopSound;
    private AnswerFeedbackDisplay feedbackDisplay;

    
    private bool isPopped = false;

    void Awake()
    {
        // 获取必要组件
        balloonExplosion = GetComponent<BalloonExplosion>();
        optionDisplay = GetComponentInChildren<BalloonOptionDisplay>();
        
        if (balloonExplosion == null)
        {
            Debug.LogError($"BalloonExplosion component missing on {gameObject.name}");
        }

        if (optionDisplay == null)
        {
            Debug.LogError($"BalloonOptionDisplay component missing on {gameObject.name}");
            // 注意：不再自动创建BalloonOptionDisplay，因为它需要特定的设置
        }

        InitializeEffects();
    }

    private void Start()
    {
        balloonExplosion = GetComponent<BalloonExplosion>();
    }

    public void SetFeedbackDisplay(AnswerFeedbackDisplay display)
    {
        feedbackDisplay = display;
    }
    
    public void SetQuizManager(QuizManager manager)
    {
        quizManager = manager;
        Debug.Log($"QuizManager set for balloon: {gameObject.name}");
    }

    private void InitializeEffects()
    {
        ParticleSystem[] effects = { hitEffect };
        foreach (var effect in effects)
        {
            if (effect != null)
            {
                var main = effect.main;
                main.playOnAwake = false;
                main.loop = false;
                effect.Stop();
            }
        }
    }

    public void OnHit(RaycastHit hit)
    {
        if (!isPopped)
        {
            TriggerExplosion(hit.point, hit.normal);
        }
    }

    private void TriggerExplosion(Vector3 hitPoint, Vector3 hitNormal)
    {
        isPopped = true;

        if (!gameObject.activeInHierarchy)
            return;

        if (quizManager == null)
        {
            Debug.LogError($"QuizManager reference is missing on balloon {gameObject.name}!");
            return;
        }
        
        // 播放击中特效
        if (hitEffect != null)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }

        // 播放正确/错误特效
            
        if (isCorrectBalloon)
        {
            feedbackDisplay.ShowCorrectFeedback();
        }
        else if (isExitBalloon)
        {
            
        }
        else
        {
            feedbackDisplay.ShowWrongFeedback();
        }

        // 播放音效
        if (balloonPopSound != null)
        {
            balloonPopSound.Play();
        }

        // 触发爆炸效果
        if (balloonExplosion != null)
        {
            balloonExplosion.Explode();
        }

        // 隐藏选项文本
        if (optionDisplay != null)
        {
            optionDisplay.SetOptionText("");  // 清空文本
        }

        // 通知QuizManager
        if (quizManager != null)
        {
            quizManager.OnBalloonShot(this);
        }
        else
        {
            Debug.LogError($"QuizManager reference is missing on balloon {gameObject.name}!");
        }
    }

    public void SetOptionText(string text)
    {
        if (optionDisplay != null)
        {
            optionDisplay.SetOptionText(text);
        }
        else
        {
            Debug.LogError($"BalloonOptionDisplay missing on {gameObject.name} when trying to set text: {text}");
        }
    }

    public void ResetBalloon()
    {
        isPopped = false;
        if (optionDisplay != null)
        {
            optionDisplay.SetOptionText("");  // 清空文本
        }
    }

}