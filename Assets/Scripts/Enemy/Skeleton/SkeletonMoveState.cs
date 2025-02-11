using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFx(10, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFx(10);
    }
}
