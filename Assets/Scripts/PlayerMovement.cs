using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxGroundSpeed = 8f;
    public float groundAcceleration = 70f;
    public float groundFriction = 8f;
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

    CharacterController controller;

    Vector3 horizontalVelocity;
    float verticalVelocity;

    Vector3 moveDir;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetMoveDir(Vector3 dir) { moveDir = dir; }

    public void UpdateMovement(bool grounded, RaycastHit groundHit, bool isSliding = false)
    {
        ApplyGravity();
        if (grounded)
            GroundMove(groundHit, isSliding);
        else
            AirMove();

        Vector3 velocity = horizontalVelocity;
        if (grounded)
        {
            velocity = Vector3.ProjectOnPlane(horizontalVelocity, groundHit.normal);
            velocity = velocity.normalized * horizontalVelocity.magnitude;
        }

        Vector3 finalVelocity = velocity + Vector3.up * verticalVelocity;
        Debug.DrawRay(transform.position, finalVelocity, Color.blue);
        controller.Move(finalVelocity * Time.deltaTime);
    }

    void GroundMove(RaycastHit groundHit, bool isSliding)
    {
        float slopeAngle = Vector3.Angle(Vector3.up, groundHit.normal);
        float frictionMultiplier = Mathf.Clamp01(1f - (slopeAngle / controller.slopeLimit));
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
            verticalVelocity = -1f;
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

    public void Jump() 
    { 
        verticalVelocity = jumpForce;
        horizontalVelocity *= 1f + jumpSpeedBoost;
    }

    public void WallJump(Vector3 normal)
    {
        horizontalVelocity = normal * wallJumpPush;
        verticalVelocity = jumpForce;
    }

    public bool GroundCheck(out RaycastHit hit)
    {
        float sphereRadius = controller.radius * 0.9f;
        float distance = (controller.height * 0.5f) - sphereRadius + 0.1f;

        return Physics.SphereCast(
            transform.position,
            sphereRadius,
            Vector3.down,
            out hit,
            distance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

    public bool CheckWall(out RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance))
            return true;
        if (Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance))
            return true;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance))
            return true;

        return false;
    }

    public Vector3 HorizontalVelocity()
    {
        return horizontalVelocity;
    }

    public void AddVelocity(Vector3 v)
    {
        horizontalVelocity += v;
    }
}
