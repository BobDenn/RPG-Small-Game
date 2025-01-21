using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool _canCreateClone; // make sure only one clone
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _canCreateClone = true;
        stateTimer = player.counterAttackDuration;

        // make sure the animator is reset
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        // counter_attack
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) 
            { 
                if (hit.GetComponent<Enemy>().CanBeStunned()) 
                {
                    stateTimer = 0.6f; // any value bigger than enemy's stun Duration
                    player.anim.SetBool("SuccessfulCounterAttack", true);

                    player.skill.parry.UseSkill();// going to use to restore health on parry
                    
                    if (_canCreateClone)
                    {
                        _canCreateClone = false;
                        player.skill.parry.MakeMirageWhenParry(hit.transform);
                        //player.skill.clone.CreateCloneWithDelay(hit.transform);
                    }
                }
            }
        }

        // if counter-attack failed, we do this
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
