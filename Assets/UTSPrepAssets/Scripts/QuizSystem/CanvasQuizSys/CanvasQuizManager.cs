using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

    public class CanvasQuizManager : MonoBehaviour
    {
        public QuestionDatabase questionDatabase; // ScriptableObject that contains the question database
        private int topicMax;
        private List<int> answeredQuestions = new List<int>(); // Track questions that have been answered in the current round
        public TextMeshProUGUI tipsText;
        public List<Toggle> toggleList;
        public TextMeshProUGUI indexText;
        public TextMeshProUGUI questionText;
        public List<TextMeshProUGUI> optionTextList;
        private int topicIndex = 0;

        public Button BtnBack;
        public Button BtnNext;
        public Button BtnTip;
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

            BtnTip.onClick.AddListener(() => Select_Answer(0));
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

            tipsText.text = "";
            indexText.text = "Question " + (topicIndex + 1) + ":";
            questionText.text = currentQuestion.QuestionText;

            // Set answer options
            for (int i = 0; i < currentQuestion.Options.Length; i++)
            {
                optionTextList[i].text = currentQuestion.GetOption(i);
            }
        }

        void Select_Answer(int index)
        {
            switch (index)
            {
                case 0: // Show tip
                    string correctAnswer = questionDatabase.GetQuestion(topicIndex).GetCorrectAnswerText();
                    tipsText.text = "<color=#FFAB08FF>The correct answer is: " + correctAnswer + "</color>";
                    break;

                case 1: // Previous question
                    if (topicIndex > 0)
                    {
                        topicIndex--;
                        LoadAnswer();
                    }
                    else
                    {
                        tipsText.text = "<color=#27FF02FF>There are no more questions ahead!</color>";
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
                        tipsText.text = "<color=#27FF02FF>Oops! This is the last question.</color>";
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
                    tipsText.text = "<color=#27FF02FF>Congratulations, you got it right!</color>";
                    correctCount++;
                }
                else
                {
                    tipsText.text = "<color=#FF0020FF>Sorry, wrong answer!</color>";
                }

                answerCount++;
                textAccuracy.text = "Accuracy: " + ((float)correctCount / answerCount * 100).ToString("f2") + "%";

                // Mark question as answered
                answeredQuestions.Add(topicIndex);

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
