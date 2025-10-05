using UnityEngine;

public class ShotgunPellet : MonoBehaviour
{
    [Header("Pellet Stats")]
    public int damage = 20;
    public float knockback = 5f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 direction, float speed)
    {
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        // Don't hit yourself
        if (hit.CompareTag("Player"))
        {
            return;
        }

        // Try to damage enemy
        EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            // Knockback
            Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDir * knockback, ForceMode2D.Impulse);
            }
        }

        // Destroy pellet on any hit
        Destroy(gameObject);
    }
}