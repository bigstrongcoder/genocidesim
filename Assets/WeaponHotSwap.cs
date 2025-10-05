using UnityEngine;

public class WeaponHotSwap : MonoBehaviour
{
    private PlayerShooting playerShooting;
    private DoublePumpShotgun doublePumpShotgun;

    void Start()
    {
        // Get references to both weapon scripts
        playerShooting = GetComponent<PlayerShooting>();
        doublePumpShotgun = GetComponent<DoublePumpShotgun>();

        // Start with PlayerShooting enabled
        SetActiveWeapon(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(2);
        }
    }

    void SetActiveWeapon(int weaponNumber)
    {
        bool usingPlayerShooting = weaponNumber == 1;

        // Enable one weapon, disable the other
        playerShooting.enabled = usingPlayerShooting;
        doublePumpShotgun.enabled = !usingPlayerShooting;

        Debug.Log(usingPlayerShooting ? "Switched to PlayerShooting" : "Switched to DoublePumpShotgun");
    }
}
