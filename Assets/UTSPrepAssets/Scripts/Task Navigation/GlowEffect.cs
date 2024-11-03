using UnityEngine;

public class GlowEffect : MonoBehaviour 
{
    [Header("Glow Settings")]
    [SerializeField] private Color glowColor = new Color(1f, 0f, 0f, 0.7f);
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 3f;
    
    [Header("Render Settings")]
    [SerializeField] private UnityEngine.Rendering.CompareFunction zTestMode = 
        UnityEngine.Rendering.CompareFunction.LessEqual;
    [SerializeField] private bool writeToDepth = false;
    [SerializeField] private int renderQueue = 3000;

    private Material glowMaterial;
    private float timeOffset;

    private void Start()
    {
        CreateGlowMaterial();
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void CreateGlowMaterial()
    {
        Shader shader = Shader.Find("Standard");
        glowMaterial = new Material(shader);

        // 设置渲染模式为透明
        glowMaterial.SetFloat("_Mode", 3);
        
        // 设置混合模式
        glowMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        glowMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        
        // 设置深度写入和测试
        glowMaterial.SetInt("_ZWrite", writeToDepth ? 1 : 0);
        glowMaterial.SetInt("_ZTest", (int)zTestMode);
        glowMaterial.renderQueue = renderQueue;
        
        // 设置关键字
        glowMaterial.DisableKeyword("_ALPHATEST_ON");
        glowMaterial.EnableKeyword("_ALPHABLEND_ON");
        glowMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        
        // 设置颜色和发光
        glowMaterial.color = glowColor;
        glowMaterial.EnableKeyword("_EMISSION");
        
        // 应用材质
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = glowMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
    }

    private void Update()
    {
        if (glowMaterial != null)
        {
            // 更新发光强度
            float emission = Mathf.Lerp(minIntensity, maxIntensity, 
                (Mathf.Sin((Time.time + timeOffset) * pulseSpeed) + 1f) / 2f);
            glowMaterial.SetColor("_EmissionColor", glowColor * emission);
        }
    }

    private void OnValidate()
    {
        if (glowMaterial != null)
        {
            glowMaterial.renderQueue = renderQueue;
            glowMaterial.SetInt("_ZWrite", writeToDepth ? 1 : 0);
            glowMaterial.SetInt("_ZTest", (int)zTestMode);
            glowMaterial.color = glowColor;
        }
    }

    private void OnDestroy()
    {
        if (glowMaterial != null)
        {
            Destroy(glowMaterial);
        }
    }
}