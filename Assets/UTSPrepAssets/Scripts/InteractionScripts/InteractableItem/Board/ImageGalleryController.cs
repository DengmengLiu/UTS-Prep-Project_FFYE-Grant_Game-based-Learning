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
        public Sprite[] imageGallery; // Array to hold the PNG images
        public TextMeshProUGUI messageText; // 添加提示文本组件
        public float messageDisplayTime = 2f; // 提示显示时间
        
        [Header("Events")]
        public UnityEvent onGalleryClose;

        private int currentIndex = 0;
        private float messageTimer = 0f;

        void Start()
        {
            // Initialize by displaying the first image
            if (imageGallery.Length > 0)
            {
                imageDisplay.sprite = imageGallery[currentIndex];
            }

            // 初始化时隐藏提示文本
            if (messageText != null)
            {
                messageText.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            // Use W key to show previous image
            if (Input.GetKeyDown(KeyCode.W))
            {
                ShowPrevImage();
            }
            
            // Use S key to show next image
            if (Input.GetKeyDown(KeyCode.S))
            {
                ShowNextImage();
            }
            
            // Use E key to close gallery
            if (Input.GetKeyDown(KeyCode.E))
            {
                CloseGallery();
            }

            // 更新提示消息的显示时间
            UpdateMessageDisplay();
        }

        // Display the next image
        void ShowNextImage()
        {
            if (imageGallery.Length == 0) return;

            if (currentIndex >= imageGallery.Length - 1)
            {
                // 已经是最后一页，显示提示
                ShowMessage("No more pages following");
                return;
            }

            currentIndex++;
            imageDisplay.sprite = imageGallery[currentIndex];
        }

        // Display the previous image
        void ShowPrevImage()
        {
            if (imageGallery.Length == 0) return;

            if (currentIndex <= 0)
            {
                // 已经是第一页，显示提示
                ShowMessage("No more pages ahead");
                return;
            }

            currentIndex--;
            imageDisplay.sprite = imageGallery[currentIndex];
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
            imageDisplay.gameObject.SetActive(false);
            if (messageText != null)
            {
                messageText.gameObject.SetActive(false);
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            onGalleryClose?.Invoke();
        }
    }
}