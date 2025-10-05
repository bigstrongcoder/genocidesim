using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public enum EnemyType { Pibble, Biggle, Wiggle, Spiddle, Big_Biggle, Mama_Pibble, Piblet, Squiggle }
    public EnemyType enemyType = EnemyType.Pibble;

    [Header("General Settings")]
    public float touchDamage = 1f;
    public float attackRange = 1.5f; // for Wiggle and Spiddle
    public float attackCooldown = 1.0f; 
    public GameObject projectilePrefab; // used by Spiddle
    public GameObject pibletPrefab;     // used by Mama Pibble and Squiggle

    private Transform player;
    private bool canAttack = true;
    private bool isStunned = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null || !canAttack) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // --- Behavior per enemy type ---
        if (enemyType == EnemyType.Pibble ||
            enemyType == EnemyType.Biggle ||
            enemyType == EnemyType.Big_Biggle ||
            enemyType == EnemyType.Mama_Pibble ||
            enemyType == EnemyType.Piblet ||
            enemyType == EnemyType.Squiggle)
        {
            // Touch damage enemies
            if (distance <= 1f)
            {
                TouchAttack();
            }
        }
        else if (enemyType == EnemyType.Wiggle)
        {
            if (distance <= attackRange && !isStunned)
            {
                StartCoroutine(WiggleAttack());
            }
        }
        else if (enemyType == EnemyType.Spiddle)
        {
            if (distance <= attackRange)
            {
                StartCoroutine(SpiddleAttack());
            }
        }
        else if (enemyType == EnemyType.Mama_Pibble || enemyType == EnemyType.Squiggle)
        {
            if (distance <= 8f)
            {
                StartCoroutine(SpawnPiblets());
            }
        }
    }

    // --- Attack Functions ---

    void TouchAttack()
    {
        // Damage the player directly if they have a health script
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)touchDamage);
        }
    }

    IEnumerator WiggleAttack()
    {
        canAttack = false;
        isStunned = true;

        // Perform a melee attack
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)touchDamage);
        }

        // Stunned for 1 second
        yield return new WaitForSeconds(1f);
        isStunned = false;

        // Cooldown after attacking
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator SpiddleAttack()
    {
        canAttack = false;

        // Spits a projectile forward
        if (projectilePrefab != null)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator SpawnPiblets()
    {
        canAttack = false;

        if (pibletPrefab != null)
        {
            Instantiate(pibletPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1.5f); // spawn every 1.5 seconds
        canAttack = true;
    }

    // Optional: if you want to handle touch attacks via collider trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyType == EnemyType.Pibble ||
            enemyType == EnemyType.Biggle ||
            enemyType == EnemyType.Big_Biggle ||
            enemyType == EnemyType.Mama_Pibble ||
            enemyType == EnemyType.Piblet ||
            enemyType == EnemyType.Squiggle)
        {
            if (collision.collider.CompareTag("Player"))
            {
                TouchAttack();
            }
        }
    }
}
