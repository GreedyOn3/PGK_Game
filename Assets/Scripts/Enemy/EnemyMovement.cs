using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : Movement
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 7f;
    [Header("Wall Climbing")]
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float wallDetectionDistance = 0.75f;
    [SerializeField] private float wallDetectionRadius = 0.3f;
    [SerializeField] private float wallCheckHeight = -0.75f;
    [SerializeField] private LayerMask wallClimbMask;

    private Rigidbody _rigidbody;
    private GameObject _player;
    private EnemyAnimation _animation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animation = GetComponent<EnemyAnimation>();

        _rigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (_animation.IsAttackPlaying()) return;

        MoveTowardsPlayer();
        _animation.SetSpeed(movementSpeed);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;
        direction.Normalize();

        Vector3 velocity = _rigidbody.linearVelocity;
        velocity.x = direction.x * movementSpeed;
        velocity.z = direction.z * movementSpeed;

        if (ShouldClimb())
        {
            velocity.x = 0f;
            velocity.z = 0f;
            velocity.y = climbSpeed;
        }

        _rigidbody.linearVelocity = velocity;
        _rigidbody.MoveRotation(Quaternion.LookRotation(direction));
    }

    private bool ShouldClimb()
    {
        Vector3 origin = transform.position + Vector3.up * wallCheckHeight;
        if (!Physics.SphereCast(origin, wallDetectionRadius, transform.forward, out RaycastHit hit, wallDetectionDistance, wallClimbMask))
            return false;

        return hit.normal.y < 0.3f;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * wallCheckHeight;

        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(origin, wallDetectionRadius);
        Gizmos.DrawLine(origin, origin + transform.forward * wallDetectionDistance);
    }
}
