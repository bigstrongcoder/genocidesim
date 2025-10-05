using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public enum EnemyType { Pibble, Biggle, Wiggle, Spiddle, Big_Biggle, Mama_Pibble, Piblet, Squiggle }
    public EnemyType patternType = EnemyType.Pibble;
    private int maxHealth = 1;
    private int currentHealth;
    public GameObject deathEffectPrefab;
    public float deathEffectDuration = 1f;

    void Start()
    {
        if (patternType == EnemyType.Pibble) {
            maxHealth = 3;
        } else if (patternType == EnemyType.Biggle) {
            maxHealth = 12;
        } else if (patternType == EnemyType.Wiggle) {
            maxHealth = 3;
        } else if (patternType == EnemyType.Spiddle) {
            maxHealth = 6;
        } else if (patternType == EnemyType.Big_Biggle) {
            maxHealth = 36;
        } else if (patternType == EnemyType.Mama_Pibble) {
            maxHealth = 36;
        } else if (patternType == EnemyType.Piblet) {
            maxHealth = 1;
        } else if (patternType == EnemyType.Squiggle) {
            maxHealth = 50;
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDuration);
        }

        Destroy(gameObject);
    }
}
