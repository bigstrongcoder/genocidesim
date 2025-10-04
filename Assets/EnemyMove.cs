using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public enum MovementType { Straight, Wave, Orbit }

    public Transform player;
    public float moveSpeed = 5f;
    public MovementType patternType = MovementType.Straight;

    [Header("Wave Settings")]
    public float frequency = 5f;
    public float magnitude = 1f;

    [Header("Orbit Settings")]
    public float orbitSpeed = 120f;   // degrees per second
    public float spiralSpeed = 1f;    

    [Header("Knockback Settings")]
    public float knockbackDrag = 5f;  

    private Rigidbody2D rb;
    private Vector2 knockback;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 aiVelocity = Vector2.zero;
        Vector2 toPlayer = (player.position - transform.position);

        if (patternType == MovementType.Straight)
        {
            aiVelocity = toPlayer.normalized * moveSpeed;
        }
        else if (patternType == MovementType.Wave)
        {
            Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x);
            float wave = Mathf.Sin(Time.time * frequency) * magnitude;
            aiVelocity = (toPlayer.normalized + perpendicular * wave).normalized * moveSpeed;
        }
        else if (patternType == MovementType.Orbit)
        {
            Vector2 tangent = new Vector2(-toPlayer.y, toPlayer.x).normalized;
            float tangentialSpeed = orbitSpeed * Mathf.Deg2Rad * toPlayer.magnitude;
            Vector2 radial = toPlayer.normalized * spiralSpeed;
            aiVelocity = radial + tangent * tangentialSpeed;
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
