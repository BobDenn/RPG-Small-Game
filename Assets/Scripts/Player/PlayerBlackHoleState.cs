using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float _flyTime = .4f;
    private bool _skillUsed;
    
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        _skillUsed = false;
        stateTimer = _flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!_skillUsed)
            {
                Debug.Log("CAST BLACKHOLE");
                _skillUsed = true;
            }
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
