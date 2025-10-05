using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public enum MovementType { Pibble, Biggle, Wiggle, Spiddle, Big_Biggle, Mama_Pibble, Piblet, Squiggle }

    public Transform player;
    public MovementType patternType = MovementType.Pibble;

    private float moveSpeed = 5f;

    [Header("Knockback Settings")]
    public float knockbackDrag = 5f;
    private Rigidbody2D rb;
    private Vector2 knockback;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb = GetComponent<Rigidbody2D>();

        // Set moveSpeed based on enemy type
        if (patternType == MovementType.Pibble) {
            moveSpeed = 4f;
        } else if (patternType == MovementType.Biggle) {
            moveSpeed = 2f;
        } else if (patternType == MovementType.Wiggle) {
            moveSpeed = 6f;
        } else if (patternType == MovementType.Spiddle) {
            moveSpeed = 2f;
        } else if (patternType == MovementType.Big_Biggle) {
            moveSpeed = 1.5f;
        } else if (patternType == MovementType.Mama_Pibble) {
            moveSpeed = 2f;
        } else if (patternType == MovementType.Piblet) {
            moveSpeed = 5f;
        } else if (patternType == MovementType.Squiggle) {
            moveSpeed = 4f;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 aiVelocity = Vector2.zero;
        Vector2 toPlayer = (player.position - transform.position).normalized;

        if (patternType == MovementType.Pibble ||
            patternType == MovementType.Biggle ||
            patternType == MovementType.Wiggle ||
            patternType == MovementType.Big_Biggle ||
            patternType == MovementType.Piblet ||
            patternType == MovementType.Squiggle)
        {
            aiVelocity = toPlayer * moveSpeed;
        }
        else if (patternType == MovementType.Spiddle || patternType == MovementType.Mama_Pibble)
        {
            aiVelocity = -toPlayer * moveSpeed;
        }

        knockback = Vector2.Lerp(knockback, Vector2.zero, knockbackDrag * Time.fixedDeltaTime);

        rb.linearVelocity = aiVelocity + knockback;
    }

    public void ApplyKnockback(Vector2 dir, float strength)
    {
        knockback = dir.normalized * strength;
    }
}
