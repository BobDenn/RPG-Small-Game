using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // all components    
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
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
    
    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;
    
    protected virtual void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    #region Damge
    public void Damge()
    {
        Debug.Log(gameObject.name + " was Damged");
    }

    #endregion


    #region Velocity

    // I can move & flip
    public void SetZeroVelocity() => rb.velocity = new Vector2(0, 0);

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
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
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    
    #endregion
}
