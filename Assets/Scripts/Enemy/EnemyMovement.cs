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
    [SerializeField] private float minWallAngle = 80f;
    [SerializeField] private LayerMask wallClimbMask;

    private Rigidbody _rigidbody;
    private GameObject _player;
    private EnemyAnimation _animation;
    private BossController _bossController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animation = GetComponent<EnemyAnimation>();
        _bossController = GetComponent<BossController>();
        _player = GameObject.FindGameObjectWithTag("Player");

        _rigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
        _animation.SetSpeed(_rigidbody.linearVelocity.magnitude);
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

        if(CanMove()) _rigidbody.linearVelocity = velocity;
        _rigidbody.MoveRotation(Quaternion.LookRotation(direction));
    }

    private bool ShouldClimb()
    {
        Vector3 origin = transform.position + Vector3.up * wallCheckHeight;
        if (!Physics.SphereCast(origin, wallDetectionRadius, transform.forward, out RaycastHit hit, wallDetectionDistance, wallClimbMask))
            return false;

        Vector3 directionToHit = (hit.point - origin).normalized;
        float rayDistance = Vector3.Distance(origin, hit.point) + 0.1f;
        Vector3 actualNormal = hit.normal;

        if (Physics.Raycast(origin, directionToHit, out RaycastHit rayHit, rayDistance, wallClimbMask))
            actualNormal = rayHit.normal;

        float surfaceAngle = Vector3.Angle(actualNormal, Vector3.up);
        if (surfaceAngle < minWallAngle)
            return false;

        return true;
    }

    private bool CanMove()
    {
        bool canMove = true;
        if (_bossController)
            canMove = _bossController.IsAttacking() || _bossController.IsSpawning();
        else
            canMove = _animation.IsAttackPlaying();

        return !canMove;
    }

    public float GetMoveSpeed() => movementSpeed;
    public void SetMoveSpeed(float speed) => movementSpeed = speed;

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * wallCheckHeight;

        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(origin, wallDetectionRadius);
        Gizmos.DrawLine(origin, origin + transform.forward * wallDetectionDistance);
    }
}
