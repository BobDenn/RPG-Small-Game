using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }
    
    
    // Initialize Statement
    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    // Change to other statements
    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
    
}
