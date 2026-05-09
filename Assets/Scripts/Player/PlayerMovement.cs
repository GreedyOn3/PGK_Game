using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : Movement
{
    public Transform visuals;

    [Header("Movement")]
    public float maxGroundSpeed = 8f;
    public float groundAcceleration = 80f;
    public float groundFriction = 8f;
    public float slopeLimit = 45f;
    public LayerMask groundMask;

    [Header("Slide")]
    public float slideMinSpeed = 1f;
    public float slideFriction = 1f;
    public float slideBoost = 5f;
    public float slopeSlideForce = 50f;

    [Header("Air")]
    public float airAcceleration = 25f;
    public float airMaxSpeed = 2f;
    public float airDrag = 0.4f;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float jumpSpeedBoost = 0.1f;
    public float gravity = -25f;

    [Header("Wall Jump")]
    public float wallCheckDistance = 0.8f;
    public float wallJumpPush = 8f;

    [Header("Surface Alignment")]
    public float groundRayDistance = 15f;
    public float groundAlignSmoothTime = 0.05f;
    public float airAlignSmoothTime = 0.4f;

    Rigidbody _rb;
    CapsuleCollider _coll;
    Vector3 _upRotationVel;
    Vector3 _lastGroundedNormal;
    bool _groundFound;

    Vector3 horizontalVelocity;
    float verticalVelocity;

    Vector3 moveDir;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<CapsuleCollider>();
        _lastGroundedNormal = Vector3.up;
    }

    public void SetMoveDir(Vector3 dir) { moveDir = dir; }

    public void UpdateMovement(bool grounded, RaycastHit groundHit, bool isSliding = false)
    {
        ApplyGravity();
        if (grounded)
            GroundMove(groundHit, isSliding);
        else
            AirMove();
        RotateTowards(moveDir, (_groundFound && verticalVelocity < 0f ? groundHit.normal : _lastGroundedNormal), grounded);

        Vector3 velocity = Vector3.zero;
        if (grounded)
            velocity = Vector3.ProjectOnPlane(horizontalVelocity, groundHit.normal).normalized * horizontalVelocity.magnitude;
        else if (isSliding)
            velocity = Vector3.ProjectOnPlane(horizontalVelocity, _lastGroundedNormal).normalized * horizontalVelocity.magnitude;
        else
            velocity = horizontalVelocity;

        Vector3 finalVelocity = velocity + Vector3.up * verticalVelocity;
        Debug.DrawRay(transform.position, finalVelocity, Color.blue);
        _rb.linearVelocity = finalVelocity;
    }

    public void RotateTowards(Vector3 dir, Vector3 up, bool grounded)
    {
        Transform toRotate = (visuals) ? visuals : transform;

        Vector3 currentUp = toRotate.up;
        Vector3 smoothUp = Vector3.SmoothDamp(
            toRotate.up,
            up,
            ref _upRotationVel,
            grounded ? groundAlignSmoothTime : airAlignSmoothTime
        ).normalized;

        Vector3 forwardDir = (dir != Vector3.zero) ? dir : toRotate.forward;
        Vector3 projectedForward = Vector3.ProjectOnPlane(forwardDir, smoothUp).normalized;

        if (projectedForward.sqrMagnitude < 0.001f)
            projectedForward = Vector3.ProjectOnPlane(toRotate.forward, smoothUp).normalized;

        toRotate.rotation = Quaternion.LookRotation(projectedForward, smoothUp);
    }

    void GroundMove(RaycastHit groundHit, bool isSliding)
    {
        float slopeAngle = Vector3.Angle(Vector3.up, groundHit.normal);
        float frictionMultiplier = Mathf.Clamp01(1f - (slopeAngle / slopeLimit));
        float friction = isSliding ? slideFriction*frictionMultiplier : groundFriction;
        ApplyFriction(friction);

        if(!isSliding)
            Accelerate(moveDir, maxGroundSpeed, groundAcceleration);
        else
        {
            Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, groundHit.normal);
            Vector3 flatSlopeDir = new Vector3(slopeDir.x, 0, slopeDir.z);

            horizontalVelocity += flatSlopeDir * slopeSlideForce * Time.deltaTime;
        }

        if (verticalVelocity < 0)
            verticalVelocity = (slopeAngle > 0f) ? 0f : -1f;
    }

    void AirMove()
    {
        Accelerate(moveDir, airMaxSpeed, airAcceleration);

        ApplyFriction(airDrag);
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }

    void Accelerate(Vector3 wishDir, float wishSpeed, float accel)
    {
        float currentSpeed = Vector3.Dot(horizontalVelocity, wishDir);
        Debug.DrawRay(transform.position, horizontalVelocity, Color.red);
        Debug.DrawRay(transform.position, wishDir, Color.green);

        float addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0)
            return;

        float accelSpeed = accel * Time.deltaTime;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        horizontalVelocity += wishDir * accelSpeed;
    }

    void ApplyFriction(float friction)
    {
        float speed = horizontalVelocity.magnitude;

        if (speed < 0.01f)
        {
            horizontalVelocity = Vector3.zero;
            return;
        }

        float drop = speed * friction * Time.deltaTime;
        float newSpeed = Mathf.Max(speed - drop, 0);

        horizontalVelocity *= newSpeed / speed;
    }

    public void Jump(Vector3 groundNormal) 
    {
        Vector3 currentSlopeVel = Vector3.ProjectOnPlane(horizontalVelocity, groundNormal).normalized * horizontalVelocity.magnitude;
        Vector3 jumpDir = Vector3.Slerp(Vector3.up, groundNormal, 0.5f).normalized;
        Vector3 finalJumpVel = currentSlopeVel + (jumpDir * jumpForce);

        horizontalVelocity *= (1f + jumpSpeedBoost);
        verticalVelocity = finalJumpVel.y;
    }

    public void WallJump(Vector3 normal)
    {
        horizontalVelocity = normal * wallJumpPush;
        verticalVelocity = jumpForce;
    }

    public bool GroundCheck(out RaycastHit hit)
    {
        if (verticalVelocity > 0.1f)
        {
            hit = new RaycastHit();
            _groundFound = false;
            return false;
        }

        float sphereRadius = _coll.radius * 0.95f;
        float distance = (_coll.height * 0.5f) - sphereRadius + 0.1f;

        bool hasFooting = Physics.CheckSphere(
            transform.position + Vector3.down * distance,
            sphereRadius,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        _groundFound = Physics.Raycast(
            transform.position,
            Vector3.down,
            out hit,
            (_coll.height * 0.5f) + groundRayDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        bool grounded = hasFooting && _groundFound && (hit.distance <= 1.3f) && (Vector3.Angle(hit.normal, Vector3.up) <= slopeLimit);
        if(grounded) _lastGroundedNormal = hit.normal;

        return grounded;
    }

    public bool CheckWall(out RaycastHit hit, out bool right)
    {
        Transform checkTransform = (visuals) ? visuals : transform;

        if (right = Physics.Raycast(transform.position, checkTransform.right, out hit, wallCheckDistance, groundMask))
            return true;
        if (Physics.Raycast(transform.position, -checkTransform.right, out hit, wallCheckDistance, groundMask))
            return true;

        return false;
    }

    public void BoostVelocity(float boost) => horizontalVelocity += horizontalVelocity.normalized * boost;
    public Vector3 GetMoveDir() => moveDir;
    public Vector3 GetHorizontalVelocity() => horizontalVelocity;
    public float GetCurrentSpeed() => horizontalVelocity.magnitude;
}
