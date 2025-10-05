using UnityEngine;

public class ShotgunPellet : MonoBehaviour
{
    [Header("Pellet Settings")]
    public float damage = 15f;
    public float lifetime = 2f;
    public float knockbackForce = 3f;

    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    [System.Obsolete]
    public void Initialize(Vector2 shootDirection, float speed)
    {
        direction = shootDirection.normalized;

        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        // Rotate pellet to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Don't hit player
        if (other.CompareTag("Player")) return;

        // Damage enemy
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage((int)damage);

            // Apply knockback
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // Destroy pellet on hit
        Destroy(gameObject);
    }
}