using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PathGuideManager : MonoBehaviour
{
    [System.Serializable]
    public class TaskPoint
    {
        public Transform targetPoint;  // 任务点位置
        public UnityEvent onReachPoint;  // 到达任务点后触发的事件
    }

    public TaskPoint[] taskPoints;  // 任务点列表
    private int currentTaskIndex = 0;  // 当前任务点索引

    private NavMeshPath navPath;  // NavMesh 路径
    private LineRenderer lineRenderer;  // 用于绘制路径的 LineRenderer

    void Start()
    {
        navPath = new NavMeshPath();
        lineRenderer = GetComponent<LineRenderer>();

        if (taskPoints.Length > 0)
        {
            UpdatePath();  // 初始化路径到第一个任务点
        }
    }

    void Update()
    {
        // 更新路径指引，显示玩家当前位置到目标点的路径
        if (taskPoints.Length > currentTaskIndex)
        {
            Vector3 playerPosition = transform.position;  // 玩家当前所在位置
            Vector3 targetPosition = taskPoints[currentTaskIndex].targetPoint.position;  // 当前任务点位置

            if (NavMesh.CalculatePath(playerPosition, targetPosition, NavMesh.AllAreas, navPath))
            {
                DrawPath(navPath);
            }
        }
    }

    void DrawPath(NavMeshPath path)
    {
        // 根据路径的拐点绘制 LineRenderer
        lineRenderer.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }
    }

    public void CompleteTask()
    {
        // 隐藏当前路径
        HidePath();

        // 切换到下一个任务点
        currentTaskIndex++;
        if (currentTaskIndex < taskPoints.Length)
        {
            UpdatePath();  // 更新到下一个任务点的路径
        }
        else
        {
            Debug.Log("所有任务点已完成！");
        }
    }

    void HidePath()
    {
        lineRenderer.positionCount = 0;  // 清空路径
    }

    void UpdatePath()
    {
        if (currentTaskIndex < taskPoints.Length)
        {
            Debug.Log($"更新路径到任务点 {currentTaskIndex}");
            Vector3 targetPosition = taskPoints[currentTaskIndex].targetPoint.position;
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, navPath);
        }
    }
}
