using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro; // 添加TMPro命名空间

namespace UTSOrientationGamePrototype
{
    public class ImageGalleryController : MonoBehaviour
    {
        [Header("Display Settings")]
        public Image imageDisplay; // Reference to the Image component
        public GameObject[] panelGallery; // Array to hold the panels
        public TextMeshProUGUI messageText; // 添加提示文本组件
        public float messageDisplayTime = 2f; // 提示显示时间
        
        [Header("UI Buttons")]
        public Button nextButton; // Button to show the next panel
        public Button prevButton; // Button to show the previous panel
        public Button closeButton; // Button to close the gallery

        [Header("Events")]
        public UnityEvent onGalleryClose;

        private int currentIndex = 0;
        private float messageTimer = 0f;

        void Start()
        {
            // Initialize by displaying the first panel
            if (panelGallery.Length > 0)
            {
                DisplayCurrentPanel();
            }

            // 初始化时隐藏提示文本
            if (messageText != null)
            {
                messageText.gameObject.SetActive(false);
            }

            // Assign button click events
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(ShowNextPanel);
            }
            if (prevButton != null)
            {
                prevButton.onClick.AddListener(ShowPrevPanel);
            }
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseGallery);
            }
        }

        void Update()
        {
            // 更新提示消息的显示时间
            UpdateMessageDisplay();
        }

        // Display the next panel
        void ShowNextPanel()
        {
            if (panelGallery.Length == 0) return;

            if (currentIndex >= panelGallery.Length - 1)
            {
                // 已经是最后一页，显示提示
                ShowMessage("No more pages following");
                return;
            }

            panelGallery[currentIndex].SetActive(false);
            currentIndex++;
            DisplayCurrentPanel();
        }

        // Display the previous panel
        void ShowPrevPanel()
        {
            if (panelGallery.Length == 0) return;

            if (currentIndex <= 0)
            {
                // 已经是第一页，显示提示
                ShowMessage("No more pages ahead");
                return;
            }

            panelGallery[currentIndex].SetActive(false);
            currentIndex--;
            DisplayCurrentPanel();
        }

        // 显示当前的panel
        void DisplayCurrentPanel()
        {
            if (panelGallery.Length > 0 && panelGallery[currentIndex] != null)
            {
                panelGallery[currentIndex].SetActive(true);
            }
        }

        // 显示提示消息
        void ShowMessage(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
                messageText.gameObject.SetActive(true);
                messageTimer = messageDisplayTime;
            }
        }

        // 更新提示消息的显示时间
        void UpdateMessageDisplay()
        {
            if (messageText != null && messageText.gameObject.activeSelf)
            {
                if (messageTimer > 0)
                {
                    messageTimer -= Time.deltaTime;
                    if (messageTimer <= 0)
                    {
                        messageText.gameObject.SetActive(false);
                    }
                }
            }
        }
        
        // Close the image gallery
        void CloseGallery()
        {
            onGalleryClose?.Invoke();
        }
    }
}
