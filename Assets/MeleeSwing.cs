using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    public float range = 3f;
    public float knockbackStrength = 10f;
    public int damage = 1;
    public LayerMask enemyLayer;
    public ParticleSystem swingEffect;

    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    private Vector2 facingDirection = Vector2.right;

    void Update()
    {
        // Update facing direction based on movement input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput.sqrMagnitude > 0.01f)
            facingDirection = moveInput.normalized;

        // Only attack if cooldown allows
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(); // attack logic and particle happen here
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        // Rotate particle to match attack direction
        if (swingEffect != null)
        {
            Vector3 facingEuler = Quaternion.LookRotation(Vector3.forward, facingDirection).eulerAngles;
            swingEffect.transform.position = transform.position;
            swingEffect.transform.rotation = Quaternion.Euler(0, 0, facingEuler.z);
            swingEffect.Play();
        }

        // Deal damage to enemies in the half-circle in front
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Vector2 toEnemy = (enemy.transform.position - transform.position).normalized;
            float angleToEnemy = Vector2.Angle(facingDirection, toEnemy);

            if (angleToEnemy <= 90f) // only hit enemies in front
            {
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.AddForce(toEnemy * knockbackStrength, ForceMode2D.Impulse);

                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null)
                    health.TakeDamage(damage);
            }
        }
    }
}
