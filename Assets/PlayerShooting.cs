using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && timer >= cooldown)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            timer = 0f;
        }
    }
}
