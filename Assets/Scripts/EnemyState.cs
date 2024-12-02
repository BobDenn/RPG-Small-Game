using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine _stateMachine;
    protected Enemy _enemy;

    protected bool triggerCalled;
    private string animBoolName;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        
    }

}
