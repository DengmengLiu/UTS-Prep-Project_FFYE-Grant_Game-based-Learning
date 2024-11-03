// ScoreManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    
    [Header("Score Settings")]
    public int correctScore = 5;    // 正确得分
    public int wrongScore = -1;     // 错误扣分
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;  // 分数显示文本
    
    private int currentScore = 0;
    
    // 单例模式访问接口
    public static ScoreManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        // 确保场景中只有一个ScoreManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        UpdateScoreDisplay();
    }
    
    // 更新分数显示
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
    
    // 添加分数
    public void AddScore(bool isCorrect)
    {
        currentScore += isCorrect ? correctScore : wrongScore;
        UpdateScoreDisplay();
    }
    
    // 获取当前分数
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    // 重置分数
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }
}

