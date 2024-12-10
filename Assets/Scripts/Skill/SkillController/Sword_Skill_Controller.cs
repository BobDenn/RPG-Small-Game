using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returningSpeed = 16;
    private Animator _anim;
    private Rigidbody2D _rb;
    private CircleCollider2D _cd;
    private Player _player;
    
    private bool _canRotate = true;
    private bool _isReturning;

    [Header("Pierce info")] 
    [SerializeField] private float pierceAmount;
    
    /*     bounce info    */
    [SerializeField] private float bounceSpeed;
    private bool _isBouncing;
    private int _amountOfBounce;
    private List<Transform> _enemyTarget; // need Initialize the list if in private state
    private int _targetIndex;
    
    
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<CircleCollider2D>();
        
    }

    public void SetupSword(Vector2 dir, float gravityScale,Player player)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravityScale;
        
        if(pierceAmount <= 0)
            _anim.SetBool("Rotate", true);
    }

    // bounce
    public void SetupBounce(bool isBouncing, int amountOfBounce)
    {
        this._isBouncing = isBouncing;
        this._amountOfBounce = amountOfBounce;
        
        //Initialize list
        _enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void ReturnSword()
    {
        // sword backs to player
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        // _rb.isKinematic = false;
        transform.parent = null;
        _isReturning = true;
    }

    private void Update()
    {
        if(_canRotate)
            transform.right = _rb.velocity;

        if (_isReturning)
        {
            // return to player
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, returningSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _player.transform.position) < 1)
                // catch the sword
                _player.CatchTheSword();
        }

        BounceLogic();
        
    }

    private void BounceLogic()
    {
        if (_isBouncing && _enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTarget[_targetIndex].position,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _enemyTarget[_targetIndex].position) < .1f)
            {
                _targetIndex++;
                _amountOfBounce--;
                
                // control amount of bounce
                if (_amountOfBounce <= 0)
                {
                    _isBouncing   = false;
                    _isReturning = true;
                }
                
                if (_targetIndex >= _enemyTarget.Count)
                    _targetIndex = 0;
            }
            
        }
    }

    // make sword stuck in enemy or ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning)
            return;
        
         // difficult
        collision.GetComponent<Enemy>()?.Damage();
        
        
        // Add enemy to our target list
        if (collision.GetComponent<Enemy>() != null)
        {
            if (_isBouncing && _enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if(hit.GetComponent<Enemy>() != null)
                        _enemyTarget.Add(hit.transform);
                }
            }
        }
        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 & collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        
        _canRotate = false;
        //collider is disabled, not gonna to touch anything else
        _cd.enabled = false;
        
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        if(_isBouncing && _enemyTarget.Count > 1)
            return;
        
        _anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }
}
