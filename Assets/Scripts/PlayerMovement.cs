using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference moveAction; // Expects a Vector2.
    public float movementSpeed = 8.0f;

    private void Update()
    {
        var movement = moveAction.action.ReadValue<Vector2>();
        var movementVector = new Vector3(movement.x, 0.0f, movement.y);
        transform.localPosition += movementVector * (movementSpeed * Time.deltaTime);
    }
}
