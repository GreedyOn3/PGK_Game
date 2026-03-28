using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7.0f;

    private Rigidbody _rigidbody;
    private GameObject _player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        var position = Vector3.MoveTowards(transform.position, _player.transform.position, movementSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(position);
    }
}
