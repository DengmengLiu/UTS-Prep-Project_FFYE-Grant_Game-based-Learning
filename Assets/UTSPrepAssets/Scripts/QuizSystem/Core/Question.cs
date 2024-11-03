using UnityEngine;
using System;

[Serializable]
public class Question
{
    [Header("Question Content")]
    [SerializeField, TextArea(3, 10)]
    [Tooltip("问题文本内容")]
    private string questionText;

    [SerializeField]
    [Tooltip("选项内容（至少2个选项）")]
    private string[] options = new string[4]; // 默认4个选项

    [SerializeField]
    [Tooltip("正确选项的索引（从0开始）")]
    private int correctOptionIndex;

    [SerializeField, TextArea(2, 5)]
    [Tooltip("答错后显示的提示信息")]
    private string hint;

    [Header("Scoring Settings")]
    [SerializeField, Min(0)]
    [Tooltip("答对获得的分数")]
    private int points = 10;

    [SerializeField, Min(0)]
    [Tooltip("答错扣除的分数")]
    private int penalty = 5;

    [Header("Time Settings")]
    [SerializeField, Min(1)]
    [Tooltip("回答问题的时间限制（秒）")]
    private float timeLimit = 10f;

    // 公开属性
    public string QuestionText => questionText;
    public string[] Options => options;
    public string Hint => hint;
    public int CorrectOptionIndex => correctOptionIndex;
    public int Points { get => points; set => points = Mathf.Max(0, value); }
    public int Penalty { get => penalty; set => penalty = Mathf.Max(0, value); }
    public float TimeLimit { get => timeLimit; set => timeLimit = Mathf.Max(1f, value); }
    public int OptionCount => options?.Length ?? 0;

    // 构造函数
    public Question() { }

    public Question(string questionText, string[] options, int correctOptionIndex, string hint = "")
    {
        this.questionText = questionText;
        this.options = options;
        this.correctOptionIndex = correctOptionIndex;
        this.hint = hint;
    }

    // 检查答案是否正确
    public bool CheckAnswer(int selectedIndex)
    {
        // 首先验证索引是否在有效范围内
        if (selectedIndex < 0 || selectedIndex >= OptionCount)
            return false;
            
        return selectedIndex == correctOptionIndex;
    }

    // 获取指定索引的选项
    public string GetOption(int index)
    {
        if (options == null || index < 0 || index >= options.Length)
            return string.Empty;
        return options[index];
    }

    // 获取正确答案文本
    public string GetCorrectAnswerText()
    {
        return GetOption(correctOptionIndex);
    }

    // 获取错误提示（仅在答错后调用）
    public string GetHint()
    {
        return string.IsNullOrEmpty(hint) ? "" : hint;
    }

    // 验证问题数据的有效性
    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(questionText))
        {
            Debug.LogError("Question text cannot be empty.");
            return false;
        }

        if (options == null || options.Length < 2)
        {
            Debug.LogError("Question must have at least 2 options.");
            return false;
        }

        // 检查选项内容
        for (int i = 0; i < options.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(options[i]))
            {
                Debug.LogError($"Option {i} cannot be empty.");
                return false;
            }
        }

        if (correctOptionIndex < 0 || correctOptionIndex >= options.Length)
        {
            Debug.LogError($"Correct option index ({correctOptionIndex}) is out of range [0, {options.Length - 1}].");
            return false;
        }

        if (timeLimit < 1f)
        {
            Debug.LogError("Time limit must be at least 1 second.");
            return false;
        }

        if (points < 0)
        {
            Debug.LogError("Points cannot be negative.");
            return false;
        }

        if (penalty < 0)
        {
            Debug.LogError("Penalty cannot be negative.");
            return false;
        }

        return true;
    }

    // 创建问题的深拷贝
    public Question Clone()
    {
        return new Question
        {
            questionText = this.questionText,
            options = (string[])this.options?.Clone(),
            correctOptionIndex = this.correctOptionIndex,
            hint = this.hint,
            points = this.points,
            penalty = this.penalty,
            timeLimit = this.timeLimit
        };
    }

    // 洗牌选项顺序（保持正确答案追踪）
    public void ShuffleOptions()
    {
        if (options == null || options.Length <= 1) return;

        string correctAnswer = options[correctOptionIndex];
        
        // Fisher-Yates 洗牌算法
        for (int i = options.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            string temp = options[randomIndex];
            options[randomIndex] = options[i];
            options[i] = temp;
        }

        // 更新正确答案的新索引
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] == correctAnswer)
            {
                correctOptionIndex = i;
                break;
            }
        }
    }

    // 用于调试的字符串表示
    public override string ToString()
    {
        return $"Question: {questionText} (Options: {OptionCount}, Correct: {correctOptionIndex}, Has Hint: {!string.IsNullOrEmpty(hint)})";
    }

#if UNITY_EDITOR
    // Unity编辑器验证
    public void OnValidate()
    {
        timeLimit = Mathf.Max(1f, timeLimit);
        points = Mathf.Max(0, points);
        penalty = Mathf.Max(0, penalty);
        
        if (options != null)
        {
            correctOptionIndex = Mathf.Clamp(correctOptionIndex, 0, options.Length - 1);
        }
    }
#endif
}