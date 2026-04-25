using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : Movement
{
    [SerializeField] private float movementSpeed = 7.0f;

    private Rigidbody _rigidbody;
    private GameObject _player;
    private EnemyAnimation _animation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animation = GetComponent<EnemyAnimation>();
    }

    private void FixedUpdate()
    {
        if (_animation.IsAttackPlaying()) return;

        var position = Vector3.MoveTowards(transform.position, _player.transform.position, movementSpeed * Time.fixedDeltaTime);
        var playerDirection = (_player.transform.position - transform.position).normalized;
        var rotation = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg;
        _rigidbody.Move(position, Quaternion.Euler(0f, rotation, 0f));
        _animation.SetSpeed(movementSpeed);
    }
}
