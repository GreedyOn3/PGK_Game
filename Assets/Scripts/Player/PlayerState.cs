using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine sm;

    public PlayerState(PlayerController p, PlayerStateMachine sm)
    {
        player = p;
        this.sm = sm;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}

public class GroundedState : PlayerState
{
    public GroundedState(PlayerController p, PlayerStateMachine sm) : base(p, sm) { }

    public override void Enter() { player.SetAnimatorValue("Grounded", true); }

    public override void Update()
    {
        if (player.CheckJumpBuffered())
        {
            player.movement.Jump();
            player.SetAnimatorValue("Jump");
            if(player.jumpParticles) player.jumpParticles.Play();
            player.ClearJumpBuffer();

            sm.ChangeState(player.AirState);
            return;
        }

        player.Move();
        player.SetAnimatorValue("Speed", Mathf.Clamp01(player.movement.GetCurrentSpeed() / player.movement.maxGroundSpeed));

        if (!player.IsGrounded())
        {
            sm.ChangeState(player.AirState);
            return;
        }

        if (player.IsSliding() && player.movement.GetHorizontalVelocity().magnitude > player.movement.slideMinSpeed)
        {
            sm.ChangeState(player.SlideState);
            return;
        }
    }
}

public class AirState : PlayerState
{
    public AirState(PlayerController p, PlayerStateMachine sm) : base(p, sm) { }

    public override void Enter() { player.SetAnimatorValue("Grounded", false); }

    public override void Update()
    {
        player.Move();

        if (player.IsGrounded())
        {
            sm.ChangeState(player.GroundedState);
            return;
        }

        if (player.CheckJumpBuffered() && player.movement.CheckWall(out RaycastHit hit, out bool right))
        {
            player.SetAnimatorValue((right) ? "Wall Jump Right" : "Wall Jump Left");
            player.movement.WallJump(hit.normal);
            player.ClearJumpBuffer();
            return;
        }
    }
}

public class SlideState : PlayerState
{
    public SlideState(PlayerController p, PlayerStateMachine sm) : base(p, sm) { }

    public override void Enter()
    {
        player.movement.BoostVelocity(player.movement.slideBoost);
        player.SetAnimatorValue("Sliding", true);
    }

    public override void Exit()
    {
        player.StopSliding();
        player.SetAnimatorValue("Sliding", false);
    }

    public override void Update()
    {
        player.Move(true);
        //float currentSpeed = player.movement.GetHorizontalVelocity().magnitude;

        if (!player.IsSliding())
        {
            sm.ChangeState(player.GroundedState);
            return;
        }

        if (!player.IsGrounded())
        {
            sm.ChangeState(player.AirState);
            return;
        }
    }
}