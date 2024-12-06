using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        // Attack logic
        if (enemy.IsPlayerDetected())
        {
            // The enemy's battle time
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                // change to attackState
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            // change to Idle
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)  
                stateMachine.ChangeState(enemy.idleState);
            
        }
        
        
        
        // follow Player
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;
        // battle move speed
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        
    }

    public override void Exit()
    {
        base.Exit();
        
    }
    // Skeleton attack cooldown
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        
        return false;
    }
}
