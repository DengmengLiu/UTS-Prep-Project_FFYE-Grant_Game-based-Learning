using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BookPageButtonController : MonoBehaviour
{
    public Button selectBtn_L;
    public Button selectBtn_R;
    public Button prevBtn;
    public Button nextBtn;
    public BookRaw bookRaw;
    private float screenWidth;
    private float mouseXPosition;

    [Header("Scene Navigation")]
    [Tooltip("Scene名称的前缀，最终场景名会是 prefix + pageNumber")]
    public string sceneNamePrefix = "Level_";
    
    [Tooltip("如果场景加载失败是否显示警告")]
    public bool showLoadWarnings = true;

    void Start()
    {
        screenWidth = Screen.width;
        
        if (selectBtn_L != null)
        {
            selectBtn_L.onClick.AddListener(() => LoadSceneForPage(bookRaw.currentPage));
        }
        
        if (selectBtn_R != null)
        {
            selectBtn_R.onClick.AddListener(() => LoadSceneForPage(bookRaw.currentPage + 1));
        }
    }

    private void Update()
    {
        mouseXPosition = Input.mousePosition.x;

        if (bookRaw != null)
        {
            int totalPageCount = bookRaw.TotalPageCount;
            int currentPage = bookRaw.currentPage;

            // 修改逻辑：第一页只显示右按钮，最后一页只显示左按钮，中间页两个按钮都可能显示
            if (currentPage == 0) // 第一页
            {
                selectBtn_L.gameObject.SetActive(false);
                prevBtn.gameObject.SetActive(false);
                selectBtn_R.gameObject.SetActive(mouseXPosition >= screenWidth / 2);
                nextBtn.gameObject.SetActive(mouseXPosition >= screenWidth / 2);
            }
            else if (currentPage == totalPageCount) // 最后一页
            {
                selectBtn_L.gameObject.SetActive(mouseXPosition < screenWidth / 2);
                prevBtn.gameObject.SetActive(mouseXPosition < screenWidth / 2);
                selectBtn_R.gameObject.SetActive(false);
                nextBtn.gameObject.SetActive(false);
            }
            else // 中间页
            {
                if (mouseXPosition < screenWidth / 2)
                {
                    selectBtn_L.gameObject.SetActive(true);
                    prevBtn.gameObject.SetActive(true);
                    selectBtn_R.gameObject.SetActive(false);
                    nextBtn.gameObject.SetActive(false);
                }
                else
                {
                    selectBtn_L.gameObject.SetActive(false);
                    nextBtn.gameObject.SetActive(true);
                    selectBtn_R.gameObject.SetActive(true);
                    prevBtn.gameObject.SetActive(false);
                }
            }
        }
    }

    public void LoadSceneForPage(int pageNumber)
    {
        string sceneName = sceneNamePrefix + pageNumber;
        
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            try
            {
                SceneManager.LoadScene(sceneName);
            }
            catch (System.Exception e)
            {
                if (showLoadWarnings)
                {
                    Debug.LogWarning($"Failed to load scene {sceneName}: {e.Message}");
                }
            }
        }
        else
        {
            if (showLoadWarnings)
            {
                Debug.LogWarning($"Scene {sceneName} does not exist in the build settings!");
            }
        }
    }

    private void OnDestroy()
    {
        if (selectBtn_L != null)
        {
            selectBtn_L.onClick.RemoveAllListeners();
        }
        
        if (selectBtn_R != null)
        {
            selectBtn_R.onClick.RemoveAllListeners();
        }
    }
}