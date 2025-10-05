using UnityEngine;

public class TaxCollectorAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRadius = 3f;
    public float knockbackForce = 10f;
    public int damage = 50;
    public float attackCooldown = 0.5f;
    public float attackAngle = 90f; // Half-angle (90 = 180 degree cone)

    [Header("Input")]
    public KeyCode attackKey = KeyCode.Space;

    [Header("Visual Feedback (Optional)")]
    public GameObject attackEffectPrefab;
    public float effectDuration = 0.3f;

    [Header("Direction Tracking")]
    public Vector2 lastFacingDirection = Vector2.right;

    private float lastAttackTime;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    [System.Obsolete]
    void Update()
    {
        // Track facing direction from movement
        UpdateFacingDirection();

        if (Input.GetKeyDown(attackKey) && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void UpdateFacingDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            lastFacingDirection = new Vector2(h, v).normalized;
        }
    }

    [System.Obsolete]
    void PerformAttack()
    {
        Debug.Log("Attack performed! Direction: " + lastFacingDirection);

        // Show visual effect if assigned
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effectDuration);
        }

        // Find all colliders in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        Debug.Log("Found " + hits.Length + " objects in radius");

        int enemiesHit = 0;

        foreach (Collider2D hit in hits)
        {
            // Skip if it's the player
            if (hit.gameObject == gameObject) continue;

            Debug.Log("Checking: " + hit.gameObject.name + " with tag: " + hit.tag);

            // Check if target has "Enemy" tag
            if (hit.CompareTag("Enemy"))
            {
                Vector2 directionToEnemy = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;

                // Check if enemy is in attack cone
                float angle = Vector2.Angle(lastFacingDirection, directionToEnemy);

                Debug.Log("Enemy " + hit.name + " angle: " + angle);

                if (angle <= attackAngle)
                {
                    enemiesHit++;
                    Debug.Log("HIT! Applying damage and knockback to " + hit.name);

                    // Apply knockback
                    Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        enemyRb.velocity = Vector2.zero; // Reset velocity first
                        enemyRb.AddForce(directionToEnemy * knockbackForce, ForceMode2D.Impulse);
                        Debug.Log("Knockback applied!");
                    }
                    else
                    {
                        Debug.LogWarning("Enemy " + hit.name + " has no Rigidbody2D!");
                    }

                    // Apply damage
                    EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                        Debug.Log("Damage applied!");
                    }
                    else
                    {
                        Debug.LogWarning("Enemy " + hit.name + " has no EnemyHealth script!");
                    }
                }
            }
        }

        Debug.Log("Total enemies hit: " + enemiesHit);
    }

    // Visualize attack range and direction in editor
    void OnDrawGizmosSelected()
    {
        // Draw attack radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw attack direction cone
        Vector3 forward = lastFacingDirection;
        Vector3 left = Quaternion.Euler(0, 0, attackAngle) * forward;
        Vector3 right = Quaternion.Euler(0, 0, -attackAngle) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward * attackRadius);
        Gizmos.DrawRay(transform.position, left * attackRadius);
        Gizmos.DrawRay(transform.position, right * attackRadius);
    }
}