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

    public override void Update()
    {
        if (player.CheckJumpBuffered())
        {
            player.movement.Jump();
            player.ClearJumpBuffer();

            sm.ChangeState(player.AirState);
            return;
        }

        player.Move();

        if (!player.IsGrounded())
        {
            sm.ChangeState(player.AirState);
            return;
        }

        if (player.IsSliding() && player.movement.HorizontalVelocity().magnitude > player.movement.slideMinSpeed)
        {
            sm.ChangeState(player.SlideState);
            return;
        }
    }
}

public class AirState : PlayerState
{
    public AirState(PlayerController p, PlayerStateMachine sm) : base(p, sm) { }

    public override void Update()
    {
        player.Move();

        if (player.IsGrounded())
        {
            sm.ChangeState(player.GroundedState);
            return;
        }

        if (player.CheckJumpBuffered() && player.movement.CheckWall(out RaycastHit hit))
        {
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
        player.movement.AddVelocity(player.movement.HorizontalVelocity().normalized * player.movement.slideBoost);
    }

    public override void Exit()
    {
        player.StopSliding();
    }

    public override void Update()
    {
        player.Move(true);
        float currentSpeed = player.movement.HorizontalVelocity().magnitude;

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