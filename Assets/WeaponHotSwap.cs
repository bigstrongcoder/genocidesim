using UnityEngine;

public class WeaponHotSwap : MonoBehaviour
{
    [Header("Weapon Scripts")]
    private PlayerShooting playerShooting;
    private DoublePumpShotgun doublePumpShotgun;

    [Header("Weapon Sprites")]
    public GameObject playerShootingGunSprite;
    public GameObject doublePumpGunSprite;

    private Transform activeGun;

    void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        doublePumpShotgun = GetComponent<DoublePumpShotgun>();

        // Start with PlayerShooting
        SetActiveWeapon(1);
    }

    void Update()
    {
        // --- Handle Hot-Swap ---
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveWeapon(2);

        // --- Make active gun follow mouse ---
        if (activeGun != null)
            AimGunAtMouse(activeGun);
    }

    void SetActiveWeapon(int weaponNumber)
    {
        bool usingPlayerShooting = weaponNumber == 1;

        playerShooting.enabled = usingPlayerShooting;
        doublePumpShotgun.enabled = !usingPlayerShooting;

        // Toggle sprites
        if (playerShootingGunSprite) playerShootingGunSprite.SetActive(usingPlayerShooting);
        if (doublePumpGunSprite) doublePumpGunSprite.SetActive(!usingPlayerShooting);

        // Set active gun transform
        activeGun = usingPlayerShooting
            ? playerShootingGunSprite?.transform
            : doublePumpGunSprite?.transform;

        Debug.Log(usingPlayerShooting ? "Switched to PlayerShooting" : "Switched to DoublePumpShotgun");
    }

    void AimGunAtMouse(Transform gun)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - gun.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip sprite vertically if aiming left
        Vector3 localScale = gun.localScale;
        localScale.y = direction.x < 0 ? -1 : 1;
        gun.localScale = localScale;
    }
}
