using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected EnemySkeleton enemy;
    protected Transform player;

    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        // cause system overload and will delete it
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        // Skeleton can find Player even though he on the behind
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
