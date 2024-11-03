using UnityEngine;

public class BalloonExplosion : MonoBehaviour
{
   [Header("References")]
    public GameObject balloonModel;           // 气球模型
    public ParticleSystem explosionParticles; // 爆炸粒子系统
    public AudioSource popAudio;              // 爆炸声音（可选）

    [Header("Explosion Settings")]
    public float explosionForce = 10f;
    public float explosionRadius = 5f;

    [Header("Fragment Settings")]
    public Material fragmentMaterial;    // 碎片材质
    [Range(3, 20)]
    public int numberOfFragments = 10;   // 碎片数量
    public float fragmentScale = 0.2f;   // 碎片大小
    public float rotationForce = 5f;     // 旋转力度

    [Header("Fragment Shapes")]
    public Vector3[] fragmentShapes = new Vector3[] {
        new Vector3(0.2f, 0.1f, 0.01f),  // 扁平状
        new Vector3(0.15f, 0.15f, 0.01f), // 圆形碎片
        new Vector3(0.1f, 0.2f, 0.01f),   // 长条状
    };

    private bool hasExploded = false;
    private void Start()
    {
        // 确保粒子系统开始时是停止的
        if (explosionParticles != null)
        {
            explosionParticles.Stop();
            // 设置粒子系统的初始位置
            explosionParticles.transform.position = transform.position;
        }
    }

     public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // 播放粒子效果
        if (explosionParticles != null)
        {
            explosionParticles.transform.position = transform.position;
            explosionParticles.Play();
        }

        // 播放声音
        if (popAudio != null)
        {
            popAudio.Play();
        }

        // 隐藏气球模型
        if (balloonModel != null)
        {
            balloonModel.SetActive(false);
        }

        // 可选：创建额外的碎片效果
        CreateFragments();

        // 延迟销毁物体
        Destroy(gameObject, 3f);
    }
    
    public void CreateFragments()
    {
        // 创建碎片容器
        GameObject fragmentContainer = new GameObject("BalloonFragments");
        fragmentContainer.transform.position = transform.position;
        
        for (int i = 0; i < numberOfFragments; i++)
        {
            // 创建碎片
            GameObject fragment = new GameObject($"Fragment_{i}");
            fragment.transform.parent = fragmentContainer.transform;
            fragment.transform.position = transform.position + Random.insideUnitSphere * 0.2f;
            
            // 随机选择碎片形状
            Vector3 randomShape = fragmentShapes[Random.Range(0, fragmentShapes.Length)];
            
            // 创建简单的平面碎片
            fragment.AddComponent<MeshFilter>().mesh = CreateFragmentMesh(randomShape);
            
            // 添加渲染器
            MeshRenderer renderer = fragment.AddComponent<MeshRenderer>();
            renderer.material = fragmentMaterial != null ? fragmentMaterial : GetComponent<MeshRenderer>().material;
            
            // 添加碰撞器
            MeshCollider collider = fragment.AddComponent<MeshCollider>();
            collider.convex = true;
            
            // 添加刚体并应用物理效果
            Rigidbody rb = fragment.AddComponent<Rigidbody>();
            Vector3 explosionDir = Random.insideUnitSphere.normalized;
            rb.AddForce(explosionDir * explosionForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * rotationForce, ForceMode.Impulse);
            
            // 随机缩放
            float randomScale = Random.Range(0.8f, 1.2f);
            fragment.transform.localScale = randomShape * fragmentScale * randomScale;
            
            // 设置随机旋转
            fragment.transform.rotation = Random.rotation;
            
            // 销毁碎片
            Destroy(fragment, 3f);
        }
        
        // 销毁容器
        Destroy(fragmentContainer, 3.1f);
    }
    
    // 创建自定义碎片网格
    private Mesh CreateFragmentMesh(Vector3 shape)
    {
        Mesh mesh = new Mesh();
        
        // 创建不规则四边形
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-shape.x, -shape.y, 0),  // 左下
            new Vector3(shape.x, -shape.y, 0),   // 右下
            new Vector3(shape.x, shape.y, 0),    // 右上
            new Vector3(-shape.x, shape.y, 0)    // 左上
        };
        
        // 添加一些随机扰动使碎片更自然
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += new Vector3(
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.02f, 0.02f)
            );
        }
        
        // 定义三角形
        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        
        // 计算法线
        Vector3[] normals = new Vector3[4];
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.forward;
        }
        
        // 设置UV坐标
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        
        // 设置网格数据
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
        
        return mesh;
    }
}