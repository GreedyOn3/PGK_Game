using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    [Header("Particles")]
    public ParticleSystem jumpParticles;
    [Header("Player Scripts")]
    public PlayerMovement movement;
    public PlayerCamera cam;
    public PlayerGathering gathering;
    public PlayerStats stats;
    [Header("Timers")]
    public float jumpBufferTime = 0.15f;

    // States
    public GroundedState GroundedState { get; private set; }
    public AirState AirState { get; private set; }
    public SlideState SlideState { get; private set; }
    PlayerStateMachine sm;

    // Is Grounded?
    bool isGrounded = false;
    RaycastHit groundHit;

    // Input
    Vector2 moveDir;
    float jumpBufferTimer;
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

        movement.maxGroundSpeed = stats.MovementSpeed;
        movement.groundAcceleration = stats.MovementSpeed*10f;

        if (gathering)
            animator.SetBool("Gathering", gathering.CheckForResources());

        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.deltaTime;
    }

    public void Move(bool sliding = false)
    {
        Vector3 move = Quaternion.Euler(0f, (cam) ? cam.playerCamera.eulerAngles.y : 0f, 0f) * new Vector3(moveDir.x, 0f, moveDir.y);

        movement.SetMoveDir(move);

        isGrounded = movement.GroundCheck(out groundHit);
        movement.UpdateMovement(isGrounded, groundHit, sliding);
    }

    public void SetAnimatorValue(string name, bool value) { if (animator) animator.SetBool(name, value); }
    public void SetAnimatorValue(string name, float value) { if (animator) animator.SetFloat(name, value); }
    public void SetAnimatorValue(string name) { if (animator) animator.SetTrigger(name); }

    public void ClearJumpBuffer() => jumpBufferTimer = 0f;
    public bool CheckJumpBuffered() => (jumpBufferTimer > 0f);
    public bool IsSliding() => sliding;
    public void StopSliding() => sliding = false;
    public bool IsGrounded() => isGrounded;

    private void OnMove(InputValue val)
    {
        moveDir = val.Get<Vector2>();

        if(moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
    }
    private void OnJump(InputValue val) { if(val.isPressed && jumpBufferTimer <= 0f) jumpBufferTimer = jumpBufferTime; }
    private void OnSlide(InputValue val) { sliding = val.isPressed; }
}