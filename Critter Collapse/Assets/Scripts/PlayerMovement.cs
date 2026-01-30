using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D pickupCollider;
    [SerializeField] float movementSpeed = 3f; //How fast we move before time and diagonal normalization
    [SerializeField] float colliderOffset = .2f;
    Vector2 movementAmount = Vector2.zero; //The amount that we adjust the player each frame, set in OnMove event
    float sprintCooldown = 0f;
    bool sprint;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pickupCollider = gameObject.GetComponent<Collider2D>();
        

    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementAmount * Time.fixedDeltaTime);
        if (movementAmount * colliderOffset != Vector2.zero) //if we're facing a direction, make our pickup area match with it
            pickupCollider.offset = movementAmount * colliderOffset; 
        //Debug.Log("Speed: " + (movementAmount * Time.fixedDeltaTime).sqrMagnitude);
        sprintCooldown = Mathf.Max(0, sprintCooldown - Time.fixedDeltaTime);
        if (sprint)
        {
            rb.MovePosition(rb.position + (movementAmount * 10f * Time.fixedDeltaTime));
            sprint = false;
            sprintCooldown = 1f;
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 playerMovementValue = (value.Get<Vector2>().normalized);
        movementAmount = playerMovementValue * movementSpeed;
    }

    void OnSprint(InputValue value)
    {
        //Debug.Log("Value.isPressed: " + value.isPressed + " Sprint Cooldown: " + sprintCooldown);
        if (value.isPressed && sprintCooldown <= 0f)
        {
            sprint = true;
        }
    }
}
