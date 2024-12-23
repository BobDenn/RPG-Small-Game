using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    public SkeletonDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enable = false;

        stateTimer = .15f;
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
            rb.velocity = new Vector2(0, 10);
    }
}
