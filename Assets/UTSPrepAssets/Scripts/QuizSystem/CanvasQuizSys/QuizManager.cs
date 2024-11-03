using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UTSOrientationGamePrototype
{
    public class QuizManager : MonoBehaviour
    {
        public QuestionDatabase questionDatabase; // ScriptableObject that contains the question database
        private int topicMax;
        private List<bool> isAnswerList = new List<bool>();

        public Text tipsText;
        public List<Toggle> toggleList;
        public Text indexText;
        public Text TM_Text;
        public List<Text> DA_TextList;
        private int topicIndex = 0;

        public Button BtnBack;
        public Button BtnNext;
        public Button BtnTip;
        public Image imageQuiz;

        public Text TextAccuracy;
        private int answerCount = 0;
        private int correctCount = 0;

        void Awake()
        {
            topicMax = questionDatabase.QuestionCount;
            for (int i = 0; i < topicMax; i++)
            {
                isAnswerList.Add(false);
            }

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
        }

        void LoadAnswer()
        {
            Question currentQuestion = questionDatabase.GetQuestion(topicIndex);

            // Reset toggles
            for (int i = 0; i < toggleList.Count; i++)
            {
                toggleList[i].isOn = false;
                toggleList[i].interactable = true;
            }

            tipsText.text = "";
            indexText.text = "Question " + (topicIndex + 1) + ":";
            TM_Text.text = currentQuestion.QuestionText;

            // Set answer options
            for (int i = 0; i < currentQuestion.Options.Length; i++)
            {
                DA_TextList[i].text = currentQuestion.GetOption(i);
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
                bool isCorrect = questionDatabase.GetQuestion(topicIndex).CorrectOptionIndex == selectedAnswerIndex;

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
                TextAccuracy.text = "Accuracy: " + ((float)correctCount / answerCount * 100).ToString("f2") + "%";

                // Disable toggles after answering
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = false;
                }
            }
        }
    }
}
