using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        // use clone skill when dash
        player.skill.dash.CreateCloneOnDashStart();
        stateTimer = player.dashDuration;
        player.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();
        // why rb.velocity.y not player.velocity.y
        // fine! it should be 0, so we don't lose our yVelocity
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
        
        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
    }

    public override void Exit()
    {
        base.Exit();
        
        player.skill.dash.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y);
        player.stats.MakeInvincible(false);
    }
}
