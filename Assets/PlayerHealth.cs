using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 25;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
