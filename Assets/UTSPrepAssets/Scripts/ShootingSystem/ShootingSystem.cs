using UnityEngine;
using UnityEngine.UI;

public class ShootingSystem : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float shootingRange = 50f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask shootingLayerMask = -1;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioSource shootingSound;

    [Header("UI")]
    [SerializeField] private Image chargeBarFill;
    private float currentCharge = 1f;

    private float nextTimeToFire = 0f;
    private Camera mainCamera;

    private void Start()
    {
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        if (shootingPoint == null)
        {
            shootingPoint = transform;
        }
        ResetChargeBar();
    }

    private void Update()
    {
        UpdateChargeBar();
        if (Time.time >= nextTimeToFire)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                nextTimeToFire = Time.time + fireRate;
                currentCharge = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        if (shootingSound != null)
        {
            shootingSound.Play();
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * shootingRange, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, shootingRange, shootingLayerMask))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");

            if (hitEffect != null)
            {
                hitEffect.transform.position = hit.point;
                hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                hitEffect.Play();
            }

            // 首先检查击中物体上的BalloonTrigger
            BalloonTrigger balloon = hit.collider.GetComponent<BalloonTrigger>();
            
            // 如果没有找到，检查父对象
            if (balloon == null)
            {
                balloon = hit.collider.GetComponentInParent<BalloonTrigger>();
                Debug.Log("Searching in parent for BalloonTrigger"); // 调试日志
            }

            if (balloon != null)
            {
                Debug.Log("Found BalloonTrigger component");
                balloon.OnHit(hit);
            }
            else
            {
                Debug.Log($"No BalloonTrigger found on {hit.collider.gameObject.name} or its parents"); // 调试日志
            }
        }
    }
     private void UpdateChargeBar()
    {
        if (currentCharge < 1f)
        {
            currentCharge += Time.deltaTime / fireRate;
            currentCharge = Mathf.Clamp01(currentCharge);
        }

        if (chargeBarFill != null)
        {
            chargeBarFill.fillAmount = currentCharge;
        }
    }

    private void ResetChargeBar()
    {
        currentCharge = 1f;
        if (chargeBarFill != null)
        {
            chargeBarFill.fillAmount = 1f;
        }
    }
}