using UnityEngine;

public class DoublePumpShotgun : MonoBehaviour
{
    [Header("References")]
    public GameObject shotgunPelletPrefab;
    public Transform firePoint;

    [Header("Shotgun Settings")]
    public int pelletsPerShot = 8;
    public float spreadAngle = 15f;
    public int shotsBeforeReload = 2;
    public float pelletSpeed = 15f;

    [Header("Recoil")]
    public float recoilForce = 8f;

    [Header("Timing")]
    public float fireRate = 0.4f;
    public float reloadTime = 1.5f;

    [Header("Input")]
    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;
    private Rigidbody2D rb;
    private Vector2 aimDirection = Vector2.right;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = shotsBeforeReload;
        mainCam = Camera.main;
    }

    void Update()
    {
        UpdateAimDirection();

        if (Input.GetKeyDown(fireKey) && Time.time >= nextFireTime && !isReloading)
        {
            if (currentAmmo > 0)
            {
                Fire();
            }
            else
            {
                StartReload();
            }
        }

        if (Input.GetKeyDown(reloadKey) && !isReloading && currentAmmo < shotsBeforeReload)
        {
            StartReload();
        }
    }

    void UpdateAimDirection()
    {
        if (mainCam != null)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            aimDirection = ((Vector2)mousePos - (Vector2)transform.position).normalized;
        }
    }

    void Fire()
    {
        currentAmmo--;
        nextFireTime = Time.time + fireRate;

        Debug.Log("BOOM! Ammo: " + currentAmmo + "/" + shotsBeforeReload);

        // Spawn all pellets
        for (int i = 0; i < pelletsPerShot; i++)
        {
            SpawnPellet();
        }

        // Apply recoil to player
        if (rb != null)
        {
            rb.AddForce(-aimDirection * recoilForce, ForceMode2D.Impulse);
        }
    }

    void SpawnPellet()
    {
        // Calculate random spread
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);
        Vector2 spreadDirection = Quaternion.Euler(0, 0, randomAngle) * aimDirection;

        // Spawn position
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        // Create pellet
        GameObject pellet = Instantiate(shotgunPelletPrefab, spawnPos, Quaternion.identity);

        // Initialize pellet
        ShotgunPellet pelletScript = pellet.GetComponent<ShotgunPellet>();
        if (pelletScript != null)
        {
            pelletScript.Initialize(spreadDirection, pelletSpeed);
        }
    }

    void StartReload()
    {
        if (!isReloading)
        {
            isReloading = true;
            Debug.Log("Reloading...");
            Invoke(nameof(FinishReload), reloadTime);
        }
    }

    void FinishReload()
    {
        currentAmmo = shotsBeforeReload;
        isReloading = false;
        Debug.Log("Loaded!");
    }
}