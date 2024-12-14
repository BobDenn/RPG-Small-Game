using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float _flyTime = .4f;
    private bool _skillUsed;
    private float _defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        _defaultGravity = player.rb.gravityScale;
        
        _skillUsed = false;
        stateTimer = _flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        
        // go up fastly and down slowly
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!_skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill())
                    _skillUsed = true;
            }
        }
        
        if(player.skill.blackHole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
        
    }

    public override void Exit()
    {
        base.Exit();
        
        player.rb.gravityScale = _defaultGravity;
        player.MakeTransparent(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
