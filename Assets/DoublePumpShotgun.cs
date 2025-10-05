using UnityEngine;

public class DoublePumpShotgun : MonoBehaviour
{
    [Header("Required Setup")]
    public GameObject pelletPrefab;
    public Transform firePoint;

    [Header("Shotgun Stats")]
    public int pelletsPerShot = 8;
    public float pelletSpeed = 20f;
    public float spreadAngle = 20f;
    public int maxAmmo = 2;

    [Header("Recoil")]
    public float recoilForce = 10f;

    [Header("Cooldowns")]
    public float shootCooldown = 0.5f;
    public float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading = false;
    private float lastShotTime = 0f;
    private Rigidbody2D playerRb;

    void Start()
    {
        currentAmmo = maxAmmo;
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Shoot with left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }

        // Reload with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    void TryShoot()
    {
        // Check if we can shoot
        if (isReloading)
        {
            Debug.Log("Still reloading!");
            return;
        }

        if (Time.time < lastShotTime + shootCooldown)
        {
            Debug.Log("Cooldown!");
            return;
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Press R to reload");
            return;
        }

        // SHOOT!
        Shoot();
    }

    void Shoot()
    {
        // Safety check
        if (pelletPrefab == null)
        {
            Debug.LogError("PELLET PREFAB IS MISSING! Drag the pellet prefab into the slot!");
            return;
        }

        currentAmmo--;
        lastShotTime = Time.time;

        Debug.Log("BOOM! Ammo: " + currentAmmo + "/" + maxAmmo);

        // Get mouse position for aiming
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mousePos - transform.position).normalized;

        // Spawn pellets with spread
        for (int i = 0; i < pelletsPerShot; i++)
        {
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            Vector2 pelletDir = Quaternion.Euler(0, 0, randomSpread) * shootDir;

            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject pellet = Instantiate(pelletPrefab, spawnPos, Quaternion.identity);

            Debug.Log("Pellet spawned at: " + spawnPos + " | Direction: " + pelletDir);

            ShotgunPellet pelletScript = pellet.GetComponent<ShotgunPellet>();
            if (pelletScript != null)
            {
                Debug.Log("Found ShotgunPellet script, launching...");
                pelletScript.Launch(pelletDir, pelletSpeed);
                Debug.Log("Pellet launched with speed: " + pelletSpeed);
            }
            else
            {
                Debug.LogError("Pellet prefab is missing ShotgunPellet script!");
            }
        }

        // Recoil pushback
        if (playerRb != null)
        {
            playerRb.AddForce(-shootDir * recoilForce, ForceMode2D.Impulse);
        }
    }

    void TryReload()
    {
        if (isReloading)
        {
            Debug.Log("Already reloading!");
            return;
        }

        if (currentAmmo >= maxAmmo)
        {
            Debug.Log("Already full!");
            return;
        }

        StartReload();
    }

    void StartReload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        Invoke("FinishReload", reloadTime);
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete! Ammo: " + currentAmmo);
    }
}