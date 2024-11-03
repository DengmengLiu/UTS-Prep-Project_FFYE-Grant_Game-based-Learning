using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestionDatabase", menuName = "Quiz/Question Database")]
public class QuestionDatabase : ScriptableObject
{
    [System.Serializable]
    public class QuizSettings
    {
        [Tooltip("默认答题时间限制（秒）")]
        public float defaultTimeLimit = 10f;
        
        [Tooltip("默认答对分数")]
        public int defaultPoints = 10;
        
        [Tooltip("默认答错扣分")]
        public int defaultPenalty = 5;
        
        [Tooltip("是否启用随机顺序")]
        public bool enableRandomOrder = true;
        
        [Tooltip("每轮使用的问题数量（0表示使用所有问题）")]
        public int questionsPerRound = 0;

        [Tooltip("默认答错提示文本")]
        [TextArea(2, 5)]
        public string defaultHint = "Please think carefully!";
    }
    
    [Header("Questions")]
    [SerializeField]
    private Question[] questions;
    
    [Header("Quiz Settings")]
    [SerializeField]
    private QuizSettings settings = new QuizSettings();
    
    // 缓存随机顺序的问题列表
    private List<Question> randomizedQuestions;
    
    // 属性访问器
    public int QuestionCount => questions?.Length ?? 0;
    public QuizSettings Settings => settings;
    
    private void OnEnable()
    {
        // 确保在加载时初始化随机问题列表
        if (settings.enableRandomOrder)
        {
            ShuffleQuestions();
        }
    }

    // 获取指定索引的问题
    public Question GetQuestion(int index)
    {
        // 验证索引范围
        if (questions == null || index < 0 || index >= questions.Length)
        {
            Debug.LogError($"Invalid question index: {index}. Total questions: {QuestionCount}");
            return null;
        }
        
        // 如果启用随机顺序但还没有随机化，先进行随机化
        if (settings.enableRandomOrder && (randomizedQuestions == null || randomizedQuestions.Count == 0))
        {
            ShuffleQuestions();
        }
        
        return settings.enableRandomOrder ? 
            randomizedQuestions[index] : questions[index];
    }
    
    // 获取本轮问题列表
    public IReadOnlyList<Question> GetQuestions()
    {
        if (questions == null || questions.Length == 0)
            return new List<Question>();
            
        if (!settings.enableRandomOrder)
            return questions.ToList();
            
        ShuffleQuestions();
        return randomizedQuestions;
    }
    
    // 随机打乱问题顺序
    public void ShuffleQuestions()
    {
        if (questions == null || questions.Length == 0)
        {
            Debug.LogError("No questions available to shuffle.");
            return;
        }
        
        randomizedQuestions = questions.ToList();
        int n = randomizedQuestions.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Question temp = randomizedQuestions[k];
            randomizedQuestions[k] = randomizedQuestions[n];
            randomizedQuestions[n] = temp;
        }
        
        // 如果设置了每轮问题数量，截取相应数量的问题
        if (settings.questionsPerRound > 0 && 
            settings.questionsPerRound < randomizedQuestions.Count)
        {
            randomizedQuestions = randomizedQuestions
                .Take(settings.questionsPerRound)
                .ToList();
        }
        
        Debug.Log($"Shuffled {randomizedQuestions.Count} questions.");
    }
    
    // 重置数据库状态
    public void Reset()
    {
        randomizedQuestions = null;
    }
    
    // 验证所有问题
    public bool ValidateQuestions()
    {
        if (questions == null || questions.Length == 0)
        {
            Debug.LogError($"{name}: No questions found in database.");
            return false;
        }
            
        bool isValid = true;
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i] == null)
            {
                Debug.LogError($"{name}: Question at index {i} is null.");
                isValid = false;
                continue;
            }
            
            if (!questions[i].Validate())
            {
                Debug.LogError($"{name}: Question at index {i} failed validation.");
                isValid = false;
            }
        }
        
        return isValid;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        // 验证设置
        settings.defaultTimeLimit = Mathf.Max(1f, settings.defaultTimeLimit);
        settings.defaultPoints = Mathf.Max(0, settings.defaultPoints);
        settings.defaultPenalty = Mathf.Max(0, settings.defaultPenalty);
        settings.questionsPerRound = Mathf.Max(0, settings.questionsPerRound);
        
        // 确保默认提示不为null
        if (string.IsNullOrEmpty(settings.defaultHint))
        {
            settings.defaultHint = "Please think carefully!";
        }
        
        // 应用默认设置到问题
        if (questions != null)
        {
            foreach (var question in questions)
            {
                if (question == null) continue;
                
                if (question.TimeLimit <= 0)
                    question.TimeLimit = settings.defaultTimeLimit;
                    
                if (question.Points <= 0)
                    question.Points = settings.defaultPoints;
                    
                if (question.Penalty < 0)
                    question.Penalty = settings.defaultPenalty;

                // 如果问题没有设置提示，使用默认提示
                if (string.IsNullOrEmpty(question.Hint))
                {
                    // 由于Hint是只读属性，需要通过其他方式设置默认值
                    // 这里假设Question类有一个SetDefaultHint方法或类似机制
                    // question.SetDefaultHint(settings.defaultHint);
                    Debug.Log($"Question in {name} has no hint, default hint will be used.");
                }
            }
        }
    }
#endif
}