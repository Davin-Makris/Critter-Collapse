using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    
    void Update()
    {
        
    }

    void OnMove(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Player Moving");
        }
    }
}
