using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackHole.blackHoleUnlocked)
        {
            if (player.skill.blackHole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("CoolDown!");
                
                return;
            }
            
            stateMachine.ChangeState(player.blackHole);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSword);

        // test counter-attack
        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);
        
        if(Input.GetKey(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);
        
        if(!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        
        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected() )
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
