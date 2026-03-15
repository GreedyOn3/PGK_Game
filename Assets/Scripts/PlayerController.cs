using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement movement;
    public float jumpBufferTime = 0.15f;

    //States
    public GroundedState GroundedState { get; private set; }
    public AirState AirState { get; private set; }
    public SlideState SlideState { get; private set; }
    PlayerStateMachine sm;

    bool isGrounded = false;
    RaycastHit groundHit;
    float jumpBufferTimer;

    //Input
    Vector2 moveDir;
    bool sliding;

    void Awake()
    {
        sm = new PlayerStateMachine();

        GroundedState = new GroundedState(this, sm);
        AirState = new AirState(this, sm);
        SlideState = new SlideState(this, sm);
    }

    void Start()
    {
        sm.Initialize(GroundedState);
    }

    void Update()
    {
        sm.Update();

        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.deltaTime;
    }

    public void Move(bool sliding = false)
    {
        Vector3 move = transform.right * moveDir.x + transform.forward * moveDir.y;

        movement.SetMoveDir(move);

        isGrounded = movement.GroundCheck(out groundHit);
        movement.UpdateMovement(isGrounded, groundHit, sliding);
    }

    public void ClearJumpBuffer() { jumpBufferTimer = 0f; }
    public bool CheckJumpBuffered() { return jumpBufferTimer > 0f; }
    public bool IsSliding() { return sliding; }
    public void StopSliding() { sliding = false; }
    public bool IsGrounded() { return isGrounded; }

    private void OnMove(InputValue val)
    {
        moveDir = val.Get<Vector2>();

        if(moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
    }
    private void OnJump(InputValue val) { if(val.isPressed) jumpBufferTimer = jumpBufferTime; }
    private void OnSlide(InputValue val) { sliding = val.isPressed; }
}