using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BalloonSpawner : MonoBehaviour
{
    [Header("Balloon Settings")]
    [SerializeField] private GameObject balloonPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 5f, 10f);
    [SerializeField] private float minDistanceBetweenBalloons = 2f;
    
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnAreaCenter;
    [SerializeField] private float yOffset = 1f;
    
    private Queue<GameObject> balloonPool;
    private List<GameObject> activeBalloons;
    private QuizManager quizManager;
     private AnswerFeedbackDisplay feedbackDisplay;
    
     private void Awake()
    {
        feedbackDisplay = FindObjectOfType<AnswerFeedbackDisplay>();
        if (spawnAreaCenter == null)
            spawnAreaCenter = transform;
            
        balloonPool = new Queue<GameObject>();
        activeBalloons = new List<GameObject>();
        
        InitializePool();
    }
    
    public void Initialize(QuizManager manager)
    {
        Debug.Log("Initializing BalloonSpawner with QuizManager");
        quizManager = manager;
        
        // 更新所有现有气球的引用
        UpdateAllBalloonsReferences();
        
        // 确保对象池已经初始化
        if (balloonPool == null || balloonPool.Count == 0)
        {
            InitializePool();
        }
    }
    
    private void UpdateAllBalloonsReferences()
    {
        // 更新池中的气球
        if (balloonPool != null)
        {
            foreach (var balloon in balloonPool)
            {
                if (balloon != null)
                {
                    UpdateBalloonReferences(balloon);
                }
            }
        }
        
        // 更新活动的气球
        if (activeBalloons != null)
        {
            foreach (var balloon in activeBalloons)
            {
                if (balloon != null)
                {
                    UpdateBalloonReferences(balloon);
                }
            }
        }
    }
    private void UpdateBalloonReferences(GameObject balloon)
    {
        BalloonTrigger trigger = balloon.GetComponent<BalloonTrigger>();
        if (trigger != null)
        {
            trigger.SetQuizManager(quizManager);
            Debug.Log($"Updated QuizManager reference for balloon: {balloon.name}");
        }
    }
    private void InitializePool()
    {
        Debug.Log($"Initializing balloon pool with size: {poolSize}");
        if (balloonPrefab == null)
        {
            Debug.LogError("Balloon prefab not assigned!");
            return;
        }

        // 清理现有的对象池
        if (balloonPool != null)
        {
            foreach (var balloon in balloonPool)
            {
                if (balloon != null)
                    Destroy(balloon);
            }
            balloonPool.Clear();
        }

        balloonPool = new Queue<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            CreateBalloon();
        }
        
        Debug.Log($"Pool initialized with {balloonPool.Count} balloons");
    }
    
    private void CreateBalloon()
    {
        if (balloonPrefab == null)
        {
            Debug.LogError("Balloon prefab not assigned!");
            return;
        }
        
        GameObject balloon = Instantiate(balloonPrefab, transform);
        balloon.name = $"Balloon_{balloonPool.Count}";
        
        // 设置引用
        UpdateBalloonReferences(balloon);
        
        balloon.SetActive(false);
        balloonPool.Enqueue(balloon);
        Debug.Log($"Created balloon: {balloon.name} with QuizManager reference");
    }
    
    public List<GameObject> SpawnBalloons(int count)
    {
        Debug.Log($"Attempting to spawn {count} balloons. Pool size: {balloonPool.Count}, Active balloons: {activeBalloons.Count}");
        
        // 确保有足够的气球在池中
        while (balloonPool.Count < count)
        {
            Debug.Log("Creating additional balloon for pool");
            CreateBalloon();
        }

        List<GameObject> spawnedBalloons = new List<GameObject>();
        List<Vector3> usedPositions = new List<Vector3>();
        
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition(usedPositions);
            if (spawnPos != Vector3.zero)
            {
                GameObject balloon = GetBalloonFromPool();
                if (balloon != null)
                {
                    balloon.transform.position = spawnPos;
                    usedPositions.Add(spawnPos);
                    spawnedBalloons.Add(balloon);
                    Debug.Log($"Spawned balloon {i + 1}/{count} at position {spawnPos}");
                }
                else
                {
                    Debug.LogError($"Failed to get balloon from pool for position {i + 1}");
                }
            }
            else
            {
                Debug.LogError($"Could not find valid spawn position for balloon {i + 1}");
            }
        }
        
        Debug.Log($"Successfully spawned {spawnedBalloons.Count} balloons");
        return spawnedBalloons;
    }
    
    private Vector3 GetValidSpawnPosition(List<Vector3> usedPositions)
    {
        const int maxAttempts = 30;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2),
                yOffset,
                Random.Range(-spawnAreaSize.z/2, spawnAreaSize.z/2)
            ) + spawnAreaCenter.position;
            
            // 检查是否与其他气球距离太近
            bool isTooClose = usedPositions.Any(pos => 
                Vector3.Distance(pos, randomPos) < minDistanceBetweenBalloons);
                
            if (!isTooClose)
            {
                return randomPos;
            }
        }
        
        Debug.LogWarning("Could not find valid spawn position, returning fallback position");
        // 如果找不到合适的位置，返回一个后备位置
        return spawnAreaCenter.position + new Vector3(0, yOffset, 0);
    }
    
    private GameObject GetBalloonFromPool()
    {
        if (balloonPool.Count == 0)
        {
            Debug.Log("Pool empty, creating new balloon");
            CreateBalloon();
        }
        
        if (balloonPool.Count > 0)
        {
            GameObject balloon = balloonPool.Dequeue();
            if (balloon == null)
            {
                Debug.LogError("Null balloon in pool, creating new one");
                CreateBalloon();
                balloon = balloonPool.Count > 0 ? balloonPool.Dequeue() : null;
            }
            
            if (balloon != null)
            {
                // 确保引用正确设置
                UpdateBalloonReferences(balloon);
                
                balloon.SetActive(true);
                activeBalloons.Add(balloon);
                
                // 确保碰撞器启用
                var collider = balloon.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }

                 // 设置BalloonTrigger的feedbackDisplay引用
                var trigger = balloon.GetComponent<BalloonTrigger>();
                if (trigger != null)
                {
                    trigger.SetFeedbackDisplay(feedbackDisplay);
                }
                
                return balloon;
            }
        }
        
        Debug.LogError("Failed to get balloon from pool");
        return null;
    }
    
     public void RecycleBalloon(GameObject balloon)
    {
        if (balloon == null) return;
        
        Debug.Log($"Recycling balloon: {balloon.name}");
        balloon.SetActive(false);
        if (activeBalloons.Contains(balloon))
        {
            activeBalloons.Remove(balloon);
        }
        if (!balloonPool.Contains(balloon))
        {
            balloonPool.Enqueue(balloon);
        }
    }
    
    public void RecycleAllBalloons()
    {
        Debug.Log($"Recycling all balloons. Active count: {activeBalloons?.Count ?? 0}");
        if (activeBalloons == null) return;
        
        var balloonsToRecycle = activeBalloons.ToList();
        foreach (var balloon in balloonsToRecycle)
        {
            if (balloon != null)
            {
                RecycleBalloon(balloon);
            }
        }
        activeBalloons.Clear();
        Debug.Log($"All balloons recycled. Pool size: {balloonPool.Count}");
    }
    
    
    private void OnDestroy()
    {
        // 清理对象池
        if (balloonPool != null)
        {
            foreach (var balloon in balloonPool)
            {
                if (balloon != null)
                    Destroy(balloon);
            }
        }
        
        if (activeBalloons != null)
        {
            foreach (var balloon in activeBalloons)
            {
                if (balloon != null)
                    Destroy(balloon);
            }
        }
    }

    private void OnValidate()
    {
        // 确保设置的值是有效的
        poolSize = Mathf.Max(1, poolSize);
        minDistanceBetweenBalloons = Mathf.Max(0.1f, minDistanceBetweenBalloons);
        spawnAreaSize = new Vector3(
            Mathf.Max(1f, spawnAreaSize.x),
            Mathf.Max(1f, spawnAreaSize.y),
            Mathf.Max(1f, spawnAreaSize.z)
        );
    }
    
    private void OnDrawGizmos()
    {
        if (spawnAreaCenter == null)
            spawnAreaCenter = transform;
            
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawCube(spawnAreaCenter.position + new Vector3(0, yOffset, 0), spawnAreaSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter.position + new Vector3(0, yOffset, 0), spawnAreaSize);
    }
}