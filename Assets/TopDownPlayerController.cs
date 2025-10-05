using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSmoothness = 10f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- Get WASD input ---
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // --- Get mouse world position ---
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        // --- Move the player ---
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // --- Rotate toward the mouse ---
        Vector2 direction = mousePos - rb.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // -90 to face up by default
        float smoothAngle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSmoothness * Time.fixedDeltaTime);
        rb.MoveRotation(smoothAngle);
    }
}
