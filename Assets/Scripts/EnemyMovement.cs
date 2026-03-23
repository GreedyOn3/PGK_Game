using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed = 7.0f;

    private Rigidbody _rigidbody;
    private GameObject _player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        var maxMovementDistance = movementSpeed * Time.fixedDeltaTime;
        var movementVector = Vector3.ClampMagnitude(_player.transform.position - transform.position, maxMovementDistance);
        _rigidbody.MovePosition(transform.localPosition + movementVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // TODO: Delete this destroy.
            Destroy(gameObject);
        }
    }
}
