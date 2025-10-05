using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Death Settings")]
    public GameObject deathEffectPrefab;
    public float deathEffectDuration = 1f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Optional: Flash effect or damage feedback here

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Spawn death effect if assigned
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDuration);
        }

        // You can add score here, drop taxes/coins, etc.

        Destroy(gameObject);
    }
}