using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    public InputActionReference moveAction; // Expects a Vector2.

    private PlayerStats _stats;

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        var movement = moveAction.action.ReadValue<Vector2>();
        var movementVector = new Vector3(movement.x, 0.0f, movement.y);
        transform.localPosition += movementVector * (_stats.movementSpeed * Time.deltaTime);
    }
}
