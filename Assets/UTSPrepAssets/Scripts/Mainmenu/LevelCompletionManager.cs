using UnityEngine;

// 用于管理关卡完成状态的单例类
public class LevelCompletionManager : MonoBehaviour
{
    private static LevelCompletionManager instance;
    public static LevelCompletionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelCompletionManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LevelCompletionManager");
                    instance = go.AddComponent<LevelCompletionManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 标记关卡为完成
    public void MarkLevelAsCompleted(int levelIndex)
    {
        PlayerPrefs.SetInt($"Level_{levelIndex}_Completed", 1);
        PlayerPrefs.Save();
    }

    // 检查关卡是否完成
    public bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level_{levelIndex}_Completed", 0) == 1;
    }

    // 重置所有进度
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteKey("Level_0_Completed"); // 气球关卡
        PlayerPrefs.DeleteKey("Level_1_Completed"); 
        PlayerPrefs.DeleteKey("Level_2_Completed");
        PlayerPrefs.DeleteKey("Level_3_Completed");
        PlayerPrefs.DeleteKey("Level_4_Completed");
        
        PlayerPrefs.Save();
    }

    // 检查是否所有关卡都已完成
    public bool AreAllLevelsCompleted()
    {
        for (int i = 0; i < 2; i++) // 假设总共有5个关卡
        {
            if (!IsLevelCompleted(i))
            {
                return false;
            }
        }
        return true;
    }
}