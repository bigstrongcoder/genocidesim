using UnityEngine;

public class WeaponHotSwap : MonoBehaviour
{
    [Header("Weapon Scripts")]
    private PlayerShooting playerShooting;
    private DoublePumpShotgun doublePumpShotgun;

    [Header("Weapon Objects")]
    public Transform weaponPivot;                // Assign this in the Inspector
    public GameObject playerShootingGunSprite;
    public GameObject doublePumpGunSprite;

    private Transform activeGun;

    void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        doublePumpShotgun = GetComponent<DoublePumpShotgun>();

        SetActiveWeapon(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveWeapon(2);

        if (weaponPivot != null)
            AimGunAtMouse(weaponPivot);
    }

    void SetActiveWeapon(int weaponNumber)
    {
        bool usingPlayerShooting = weaponNumber == 1;

        playerShooting.enabled = usingPlayerShooting;
        doublePumpShotgun.enabled = !usingPlayerShooting;

        if (playerShootingGunSprite) playerShootingGunSprite.SetActive(usingPlayerShooting);
        if (doublePumpGunSprite) doublePumpGunSprite.SetActive(!usingPlayerShooting);

        activeGun = usingPlayerShooting
            ? playerShootingGunSprite?.transform
            : doublePumpGunSprite?.transform;
    }

    void AimGunAtMouse(Transform pivot)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - pivot.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pivot.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip gun sprite without altering scale
        if (activeGun != null)
        {
            SpriteRenderer sr = activeGun.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.flipY = direction.x < 0;
        }
    }
}
