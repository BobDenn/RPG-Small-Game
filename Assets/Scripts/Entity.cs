using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // all components    
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStatus status { get; private set; }
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
        status = GetComponent<CharacterStatus>();
        cd = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    
    public void WasDamaged()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
        
        //Debug.Log(gameObject.name + " was Damaged");
    }

    protected virtual IEnumerator HitKnockBack()
    {
        IsKnocked = true;
        
        rb.velocity = new Vector2(knockBackDirection.x * -facingDir, knockBackDirection.y);

        yield return new WaitForSeconds(knockBackDuration);
        IsKnocked = false;

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
    
    // disappear
    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
        
    }
    
    public virtual void Die()
    {
        // nothing
    }
}
