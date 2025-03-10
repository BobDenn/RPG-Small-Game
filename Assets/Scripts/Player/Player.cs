using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity 
    // player's movement
{
    [Header("Attack details")] 
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    
    public bool isBusy { get; private set; }
    [Header("Move info")] 
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float swordReturnImpact;
    public float defaultMoveSpeed;
    public float defaultJumpForce;

    [Header("Dash info")] 
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }


    public SkillManager skill { get; private set; }
    public GameObject   sword { get; private set; }
    public PlayerFX fx { get; private set; }

    // all player's states
    #region States  
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword {  get; private set; }
    public PlayerBlackHoleState blackHole { get; private set; }

    public PlayerDeadState deadState {get; private set;}

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine,  "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump  = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword   = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole  = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<PlayerFX>();
        
        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if(Time.timeScale == 0)
            return;
        
        base.Update();
        
        stateMachine.currentState.Update();
        // use dash
        CheckForDashInput();

        // crystal skill
        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();

        // use Flask
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }
    // 寒冰减速效果
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);

    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }
    // only one sword
    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    
    // to control sequence of attack 
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    
    // Dash ability
    public void CheckForDashInput()
    {
        // other condition
        if (IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;
        
        // bug - fixed
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;
            
            stateMachine.ChangeState(dashState);
        }
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    protected override void SetupZeroKnockBackPower()
    {
        knockBackDirection = new Vector2(0, 0);
    }
}
