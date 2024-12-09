using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returningSpeed = 12;
    private Animator _anim;
    private Rigidbody2D _rb;
    private CircleCollider2D _cd;
    private Player _player;
    
    private bool _canRotate = true;
    private bool _isReturning;

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
        _anim.SetBool("Rotate", true);
    }

    public void ReturnSword()
    {
        // fly to player
        _rb.isKinematic = false;
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
                _player.ClearTheSword();
        }
    }

    // make sword stuck in enemy or ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _anim.SetBool("Rotate", false);
        _canRotate = false;
        //collider is disabled, not gonna to touch anything else
        _cd.enabled = false;
        
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        transform.parent = collision.transform;
    }
    
}
