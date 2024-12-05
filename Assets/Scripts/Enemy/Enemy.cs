using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    // the variable to recognise player
    public LayerMask whatIsPlayer;
    
    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;

    [Header("Attack info")]
    public float battleTime;
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;
    
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        // setup state machine
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
        
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    
    // To find Player and enter battle state
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
        
    }
}
