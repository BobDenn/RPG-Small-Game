using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // all components    
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd {get; private set;}

    #endregion
    
    [Header("Collision info")]
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;
    [Space]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    

    [Header("Knock Back info")] 
    [SerializeField] protected Vector2 knockBackDirection;
    [SerializeField] protected float knockBackDuration;
    protected bool IsKnocked;

    public int knockBackDir {get; private set;}
    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    public System.Action OnFlipped;
    
    protected virtual void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    // chill ailment effect
    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        // nothing here dude
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }
    public void WasDamaged() => StartCoroutine("HitKnockBack");
        
    //Debug.Log(gameObject.name + " was Damaged");
    public virtual void SetupKnockBackDir(Transform damageDir)
    {
        if (damageDir.position.x > transform.position.x)
            knockBackDir = -1;
        else if (damageDir.position.x < transform.position.x)
            knockBackDir = 1;
    }
    public void SetupKnockBackPower(Vector2 knockBack) => knockBackDirection = knockBack;
    
    protected virtual IEnumerator HitKnockBack()
    {
        IsKnocked = true;
        
        rb.velocity = new Vector2(knockBackDirection.x * knockBackDir, knockBackDirection.y);

        yield return new WaitForSeconds(knockBackDuration);
        IsKnocked = false;
        SetupZeroKnockBackPower();
    }

    protected virtual void SetupZeroKnockBackPower()
    {
        
    }

    #region Velocity

    // I can move & flip
    public void SetZeroVelocity()
    {
        if(IsKnocked)
            return;
        
        rb.velocity = new Vector2(0, 0);
    } 

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(IsKnocked)
            return;
        
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    #endregion
    
    #region Collision
    // to check ground and wall
    public virtual bool IsGroundDetected() =>  
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }   

    #endregion
    
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
        if(OnFlipped != null)
            OnFlipped();
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
            Flip();
        else if (x < 0 && facingRight)
            Flip();
    }
    
    #endregion
    
    public virtual void Die()
    {
        // nothing
    }
}
