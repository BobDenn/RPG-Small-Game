using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine {  get; private set; }

    public PlayerIdleState idleMachine { get; private set; }
    public PlayerMoveState moveMachine { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleMachine = new PlayerIdleState(this, stateMachine, "Idle");
    }
}
