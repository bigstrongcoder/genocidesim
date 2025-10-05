using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public enum MovementType { Pibble, Biggle, Wiggle, Spiddle, Big_Biggle, Mama_Pibble, Piblet, Squiggle }

    public Transform player;
    private float moveSpeed = 5f;
    public MovementType patternType = MovementType.Pibble;

    [Header("Knockback Settings")]
    public float knockbackDrag = 5f;

    private Rigidbody2D rb;
    private Vector2 knockback;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        // ✅ Fixed: you wrote EnemyType instead of MovementType, and floats need 'f'
        if (patternType == MovementType.Pibble)
        {
            moveSpeed = 4f;
        }
        else if (patternType == MovementType.Biggle)
        {
            moveSpeed = 2f;
        }
        else if (patternType == MovementType.Wiggle)
        {
            moveSpeed = 6f;
        }
        else if (patternType == MovementType.Spiddle)
        {
            moveSpeed = 2f;
        }
        else if (patternType == MovementType.Big_Biggle)
        {
            moveSpeed = 1.5f;
        }
        else if (patternType == MovementType.Mama_Pibble)
        {
            moveSpeed = 2f;
        }
        else if (patternType == MovementType.Piblet)
        {
            moveSpeed = 5f;
        }
        else if (patternType == MovementType.Squiggle)
        {
            moveSpeed = 4f;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 aiVelocity = Vector2.zero;
        Vector2 toPlayer = (player.position - transform.position);

        if (patternType == MovementType.Pibble ||
            patternType == MovementType.Biggle ||
            patternType == MovementType.Wiggle ||
            patternType == MovementType.Big_Biggle ||
            patternType == MovementType.Piblet ||
            patternType == MovementType.Squiggle)
        {
            aiVelocity = toPlayer.normalized * moveSpeed;
        }
        else if (patternType == MovementType.Spiddle || patternType == MovementType.Mama_Pibble)
        {
            aiVelocity = (-toPlayer).normalized * moveSpeed; // opposite direction
        }

        // Apply knockback decay
        knockback = Vector2.Lerp(knockback, Vector2.zero, knockbackDrag * Time.fixedDeltaTime);

        // Combine movement and knockback
        rb.linearVelocity = aiVelocity + knockback;
    }

    public void ApplyKnockback(Vector2 dir, float strength)
    {
        knockback = dir.normalized * strength;
    }
}
