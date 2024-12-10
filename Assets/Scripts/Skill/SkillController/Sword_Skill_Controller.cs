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

    /*     bounce info    */
    [SerializeField] private float bounceSpeed;
    private bool _isBouncing;
    private int _amountOfBounce;
    private List<Transform> _enemyTarget; // need Initialize the list if in private state
    private int _targetIndex;
    
    [Header("Pierce info")] 
    private float _pierceAmount;

    [Header("Spin info")] 
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private bool _wasStopped;
    private bool _isSpinning;

    private float _hitTimer;
    private float _hitCooldown;

    private float _spinDir;
    
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<CircleCollider2D>();
        
    }

    #region Setup Stuff
    
    public void SetupSword(Vector2 dir, float gravityScale,Player player)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravityScale;
        
        if(_pierceAmount <= 0)
            _anim.SetBool("Rotate", true);
        // ???
        _spinDir = Mathf.Clamp(_rb.velocity.x, -1, 1);
    }

    // bounce
    public void SetupBounce(bool isBouncing, int amountOfBounce)
    {
        this._isBouncing = isBouncing;
        this._amountOfBounce = amountOfBounce;
        
        //Initialize list
        _enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int pierceAmount)
    {
        _pierceAmount = pierceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        _isSpinning = isSpinning;
        _maxTravelDistance = maxTravelDistance;
        _spinDuration = spinDuration;
        _hitCooldown = hitCooldown;
    }
    
    #endregion
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
        
        /*-----------spin-----------*/
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (_isSpinning)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStopped)
            {
                StopWhenSpinning();
            }
            // stop and go back to player
            if (_wasStopped)
            {
                _spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDir, transform.position.y), 1.5f * Time.deltaTime);
                
                if (_spinTimer < 0)
                {
                    _isReturning = true;
                    _isSpinning = false;
                }

                _hitTimer -= Time.deltaTime;
                if (_hitTimer < 0)
                {
                    // continue to damage enemy
                    _hitTimer = _hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            hit.GetComponent<Enemy>().WasDamaged();
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        _wasStopped = true;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _spinDuration;
    }

    private void BounceLogic()
    {
        if (_isBouncing && _enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTarget[_targetIndex].position,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _enemyTarget[_targetIndex].position) < .1f)
            {
                _enemyTarget[_targetIndex].GetComponent<Enemy>().WasDamaged();
                
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
        collision.GetComponent<Enemy>()?.WasDamaged();
        
        
        // Add enemy to our target list
        SetupTargetsForBounce(collision);
        
        StuckInto(collision);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
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
    }

    private void StuckInto(Collider2D collision)
    {
        if (_pierceAmount > 0 & collision.GetComponent<Enemy>() != null)
        {
            _pierceAmount--;
            return;
        }

        if (_isSpinning)
        {
            // stop when touch  first enemy
            StopWhenSpinning();
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
