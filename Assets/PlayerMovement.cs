using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f; //Max move speed
    public float acceleration = 15f; //How fast a player gets to top speed
    public float deceleration = 20f; //How fast a player slows down to 0

    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Vector2 moveInput;

    void Start()
    {
        //Get Rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Get inputs WASD or Arrow Keys
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        //Apply Velocity
        Vector2 targetVelocity = moveInput * moveSpeed;

        //IDK what ts does so W google/yt
        if (Vector2.Dot(targetVelocity, currentVelocity) < 0 && moveInput != Vector2.zero)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = currentVelocity;
    }
}
