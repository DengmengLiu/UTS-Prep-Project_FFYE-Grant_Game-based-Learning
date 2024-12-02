using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestionDatabase questionDatabase;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private BalloonSpawner balloonSpawner;
    
    [Header("Game Settings")]
    [SerializeField] private float nextQuestionDelay = 2f;
    [SerializeField] private int correctPoints = 10;
    [SerializeField] private int wrongPoints = 5;
    [SerializeField] private float hintDisplayTime = 3f;
    [SerializeField] private int maxWrongAttempts = 2;

    [Header("Exit Settings")]
    [SerializeField] private string sceneToLoad = "MainMenu";
    [SerializeField] private Vector3 exitBalloonPosition = new Vector3(0, 2, 5);

    private Question currentQuestion;
    private int currentQuestionIndex = 0;
    private int totalScore = 0;
    private int wrongAttempts = 0;
    private List<GameObject> currentBalloons;
    private bool canAnswer = true;
    private bool isWaitingForNextQuestion = false;
    private bool isQuizCompleted = false;
    private int currentOptionsCount = 0; // 新增：跟踪当前题目的选项数量
    private void Start()
    {
        if (!ValidateComponents()) 
        {
            enabled = false;
            return;
        }
        InitializeQuiz();
    }

    private void InitializeQuiz()
    {
        balloonSpawner.Initialize(this);
        ResetQuizState();
        LoadNextQuestion();
    }

    private void ResetQuizState()
    {
        wrongAttempts = 0;
        canAnswer = true;
        isWaitingForNextQuestion = false;
        currentOptionsCount = 0; // 重置选项数量
        UpdateScoreDisplay();
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
    }

     private void LoadNextQuestion()
    {
        Debug.Log($"LoadNextQuestion called, index: {currentQuestionIndex}");
        if (currentQuestionIndex >= questionDatabase.QuestionCount)
        {
            Debug.Log("No more questions, ending quiz");
            EndQuiz();
            return;
        }

        // 在加载新题目前确保清理所有现有气球
        if (currentBalloons != null)
        {
            balloonSpawner.RecycleAllBalloons();
            currentBalloons.Clear();
        }

        ResetQuizState();
        currentQuestion = questionDatabase.GetQuestion(currentQuestionIndex);
        
        if (currentQuestion == null)
        {
            Debug.LogError($"Failed to load question {currentQuestionIndex}");
            EndQuiz();
            return;
        }
        
        currentOptionsCount = currentQuestion.OptionCount;
        DisplayQuestion(currentQuestion);
    }
     private void DisplayQuestion(Question question)
    {
        questionText.text = question.QuestionText;
        question.ShuffleOptions();
        SpawnBalloonsForOptions(question);
    }
    private void SpawnBalloonsForOptions(Question question)
    {
        // 确保在生成新气球前回收所有现有气球
        balloonSpawner.RecycleAllBalloons();
        
        // 使用记录的选项数量来生成气球
        currentBalloons = balloonSpawner.SpawnBalloons(currentOptionsCount);
        
        for (int i = 0; i < currentBalloons.Count; i++)
        {
            GameObject balloon = currentBalloons[i];
            string optionText = question.GetOption(i);
            
            var trigger = balloon.GetComponent<BalloonTrigger>();
            if (trigger != null)
            {
                trigger.isCorrectBalloon = (i == question.CorrectOptionIndex);
            }

            var display = balloon.GetComponent<BalloonOptionDisplay>();
            if (display != null)
            {
                display.SetOptionText(optionText);
            }
        }
    }

    public void OnBalloonShot(BalloonTrigger balloon)
    {
        if (isQuizCompleted && balloon.isExitBalloon)
        {
            HandleExitBalloon();
            return;
        }

        if (!canAnswer || isWaitingForNextQuestion) 
        {
            return;
        }

        if (balloon.isCorrectBalloon)
        {
            HandleCorrectAnswer();
        }
        else
        {
            HandleWrongAnswer(balloon.gameObject);
        }
    }


    private void HandleCorrectAnswer()
    {
        Debug.Log("Correct answer!");
        totalScore += correctPoints;
        UpdateScoreDisplay();
        canAnswer = false;
        isWaitingForNextQuestion = true;
        
        if (currentQuestionIndex >= questionDatabase.QuestionCount - 1)
        {
            Debug.Log("Last question answered correctly, ending quiz");
            Invoke(nameof(EndQuiz), nextQuestionDelay);
        }
        else
        {
            Debug.Log("Moving to next question after delay");
            Invoke(nameof(ProceedToNextQuestion), nextQuestionDelay);
        }
    }

    private void HandleWrongAnswer(GameObject wrongBalloon)
    {
        wrongAttempts++;
        totalScore -= wrongPoints;
        UpdateScoreDisplay();

        Debug.Log($"Wrong attempt {wrongAttempts} on question {currentQuestionIndex}");

        if (wrongAttempts >= maxWrongAttempts)
        {
            canAnswer = false;
            isWaitingForNextQuestion = true;
            ShowHint($"{currentQuestion.GetHint()}");
            
            balloonSpawner.RecycleAllBalloons();
            
            // 检查是否是最后一题
            if (currentQuestionIndex >= questionDatabase.QuestionCount - 1)
            {
                Debug.Log("Last question failed, scheduling quiz end");
                // 使用协程来处理最后一题的结束流程
                StartCoroutine(HandleLastQuestionFailure());
            }
            else
            {
                Debug.Log("Not last question, proceeding to next");
                Invoke(nameof(ProceedToNextQuestion), hintDisplayTime);
            }
        }
    }

     private System.Collections.IEnumerator HandleLastQuestionFailure()
    {
        Debug.Log("Starting last question failure sequence");
        // 等待提示显示时间
        yield return new WaitForSeconds(hintDisplayTime);
        
        Debug.Log("Hint time finished, ending quiz");
        // 确保在主线程中执行
        HideHint();
        EndQuiz();
    }


    private void ShowHint(string message)
    {
        if (hintText != null)
        {
            Debug.Log($"Showing hint: {message}");
            hintText.text = message;
            hintText.gameObject.SetActive(true);
        }
    }

    private void HideHint()
    {
        if (hintText != null)
        {
            Debug.Log("Hiding hint");
            hintText.gameObject.SetActive(false);
        }
    }

    private void ProceedToNextQuestion()
    {
        Debug.Log("ProceedToNextQuestion called");
        // 确保提示文本被隐藏
        HideHint();
        
        currentQuestionIndex++;
        LoadNextQuestion();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {totalScore}";
    }

    private void EndQuiz()
    {
        Debug.Log("EndQuiz called");
        isQuizCompleted = true;
        
        // 确保提示文本被隐藏
        HideHint();
        
        questionText.text = $"Quiz Completed!\nFinal Score: {totalScore}";
        balloonSpawner.RecycleAllBalloons();
        canAnswer = false;

        Debug.Log("Spawning exit balloon immediately");
        // 直接生成Exit气球，不再使用Invoke
        SpawnExitBalloon();
    }

   private void SpawnExitBalloon()
    {
        Debug.Log("SpawnExitBalloon called");
        // 确保所有现有气球都被回收
        balloonSpawner.RecycleAllBalloons();
        
        List<GameObject> exitBalloons = balloonSpawner.SpawnBalloons(1);
        if (exitBalloons != null && exitBalloons.Count > 0)
        {
            Debug.Log("Exit balloon spawned successfully");
            GameObject exitBalloon = exitBalloons[0];
            exitBalloon.transform.position = exitBalloonPosition;

            var display = exitBalloon.GetComponent<BalloonOptionDisplay>();
            if (display != null)
            {
                display.SetOptionText("Exit");
            }

            var trigger = exitBalloon.GetComponent<BalloonTrigger>();
            if (trigger != null)
            {
                trigger.isExitBalloon = true;
                trigger.isCorrectBalloon = false;
            }
        }
        else
        {
            Debug.LogError("Failed to spawn exit balloon!");
        }
    }
    private void HandleExitBalloon()
    {
        LevelCompletionManager.Instance.MarkLevelAsCompleted(0);
        Debug.Log($"Current PlayerPrefs values: Level_0_Completed = {PlayerPrefs.GetInt("Level_0_Completed", 0)}, Level_1_Completed = {PlayerPrefs.GetInt("Level_1_Completed", 0)}");
        StartCoroutine(LoadNextScene());
    }

    private System.Collections.IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneToLoad);
    }
    
    private bool ValidateComponents()
    {
        if (questionDatabase == null || questionDatabase.QuestionCount == 0)
        {
            Debug.LogError("Question Database is missing or empty!");
            return false;
        }
        
        if (questionText == null || scoreText == null)
        {
            Debug.LogError("Required UI components are missing!");
            return false;
        }
        
        if (balloonSpawner == null)
        {
            Debug.LogError("Balloon Spawner is missing!");
            return false;
        }
        
        return true;
    }
}