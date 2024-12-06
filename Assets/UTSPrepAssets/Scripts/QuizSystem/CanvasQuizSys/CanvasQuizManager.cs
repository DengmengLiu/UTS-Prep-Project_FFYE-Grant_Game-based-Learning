using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

    public class CanvasQuizManager : MonoBehaviour
    {
        public QuestionDatabase questionDatabase; // ScriptableObject that contains the question database
        private int topicMax;
        private List<int> answeredQuestions = new List<int>(); // Track questions that have been answered in the current round
        public TextMeshProUGUI ansText;
        public List<Toggle> toggleList;
        public TextMeshProUGUI indexText;
        public TextMeshProUGUI questionText;
        public Image questionImage;
        public List<TextMeshProUGUI> optionTextList;
        private int topicIndex = 0;

        public Button BtnBack;
        public Button BtnNext;
        public Button BtnAns;
        public Button BtnSubmit;
        public Image imageQuiz;

        public TextMeshProUGUI textAccuracy;
        public GameObject scoreBoard; // Scoreboard UI to display results at the end
        public TextMeshProUGUI scoreBoardText; // Text to show detailed results

        private int answerCount = 0;
        private int correctCount = 0;

        void Awake()
        {
            topicMax = questionDatabase.QuestionCount;
            ResetQuiz();
        }

        void OnEnable()
        {
            ResetQuiz();
            LoadAnswer();
        }

        void Start()
        {
            toggleList[0].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn, 0));
            toggleList[1].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn, 1));
            toggleList[2].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn, 2));
            toggleList[3].onValueChanged.AddListener((isOn) => AnswerRightWrongJudgment(isOn, 3));

            BtnAns.onClick.AddListener(() => Select_Answer(0));
            BtnBack.onClick.AddListener(() => Select_Answer(1));
            BtnNext.onClick.AddListener(() => Select_Answer(2));
            BtnSubmit.onClick.AddListener(SubmitQuiz);

            BtnSubmit.gameObject.SetActive(false);
        }

        void ResetQuiz()
        {
            answeredQuestions.Clear();
            topicIndex = 0;
            answerCount = 0;
            correctCount = 0;
            scoreBoard.SetActive(false);
            BtnSubmit.gameObject.SetActive(false);
        }

    void LoadAnswer()
    {
        Question currentQuestion = questionDatabase.GetQuestion(topicIndex);

        // Reset toggles
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].isOn = false;
            toggleList[i].interactable = !answeredQuestions.Contains(topicIndex);
        }
        BtnAns.gameObject.SetActive(false);

        // 标记已回答的问题
        if (answeredQuestions.Contains(topicIndex))
        {
            indexText.text = "<color=#27FF02FF>Question " + (topicIndex + 1) + " (Answered):</color>";
        }
        else
        {
            indexText.text = "Question " + (topicIndex + 1) + ":";
        }

        ansText.text = "";
        questionText.text = currentQuestion.QuestionText;

        // 设置答案选项
        for (int i = 0; i < currentQuestion.Options.Length; i++)
        {
            optionTextList[i].text = currentQuestion.GetOption(i);
        }

        // 设置问题图片
        if (currentQuestion.QuestionImage != null)
        {
            questionImage.gameObject.SetActive(true);
            questionImage.sprite = currentQuestion.QuestionImage;
        }
        else
        {
            questionImage.gameObject.SetActive(false); // 隐藏图片
        }
    }


    void Select_Answer(int index)
        {
            switch (index)
            {
                case 0: // Show tip
                    string correctAnswer = questionDatabase.GetQuestion(topicIndex).GetCorrectAnswerText();
                    ansText.text = "<color=#FFAB08FF>The correct answer is: " + correctAnswer + "</color>";
                    break;

                case 1: // Previous question
                    if (topicIndex > 0)
                    {
                        topicIndex--;
                        LoadAnswer();
                    }
                    else
                    {
                        ansText.text = "<color=#27FF02FF>There are no more questions ahead!</color>";
                    }
                    break;

                case 2: // Next question
                    if (topicIndex < topicMax - 1)
                    {
                        topicIndex++;
                        LoadAnswer();
                    }
                    else
                    {
                        ansText.text = "<color=#27FF02FF>Oops! This is the last question.</color>";
                    }
                    break;
            }
        }

        void AnswerRightWrongJudgment(bool isOn, int selectedAnswerIndex)
        {
            if (isOn)
            {
                Question currentQuestion = questionDatabase.GetQuestion(topicIndex);
                bool isCorrect = currentQuestion.CorrectOptionIndex == selectedAnswerIndex;

                if (isCorrect)
                {
                    ansText.text = "<color=#27FF02FF>Congratulations, you got it right!</color>";
                    correctCount++;
                }
                else
                {
                    ansText.text = "<color=#FF0020FF>Sorry, wrong answer!</color>";
                }

                answerCount++;
                textAccuracy.text = "Accuracy: " + ((float)correctCount / answerCount * 100).ToString("f2") + "%";

                // Mark question as answered
                answeredQuestions.Add(topicIndex);

                // Enable BtnTip after answering
                BtnAns.gameObject.SetActive(true);

                // Disable toggles after answering
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = false;
                }

                // Show submit button if all questions are answered
                if (answeredQuestions.Count == topicMax)
                {
                    BtnSubmit.gameObject.SetActive(true);
                }
            }
        }

        void SubmitQuiz()
        {
            ShowScoreBoard();
        }

        void ShowScoreBoard()
        {
            scoreBoard.SetActive(true);
            scoreBoardText.text = "Quiz Completed!\n\n" +
                                 "Total Questions: \t\t" + topicMax + "\n" +
                                 "Correct Answers: \t" + correctCount + "\n" +
                                 "Accuracy: \t\t\t" + ((float)correctCount / answerCount * 100).ToString("f2") + "%";
        }
}
