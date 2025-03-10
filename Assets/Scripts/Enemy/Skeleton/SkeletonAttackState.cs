using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemy;


    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFx(11, null);
    }

    public override void Update()
    {
        base.Update();
        
        enemy.SetZeroVelocity();
        
        // time to fight
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFx(11);
        enemy.lastTimeAttacked = Time.time;
    }
}
