using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float movementSpeed = 3; //How fast we move before time and diagonal normalization
    Vector2 movementAmount = Vector2.zero; //The amount that we adjust the player each frame, set in OnMove event
    float sprintCooldown = 0f;
    bool sprint;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        

    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementAmount * Time.fixedDeltaTime);
        Debug.Log("Speed: " + (movementAmount * Time.fixedDeltaTime).sqrMagnitude);
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
        Debug.Log("Value.isPressed: " + value.isPressed + " Sprint Cooldown: " + sprintCooldown);
        if (value.isPressed && sprintCooldown <= 0f)
        {
            sprint = true;
        }
    }
}
