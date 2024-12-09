using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;
    private CircleCollider2D _cd;
    private Player _player;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<CircleCollider2D>();
        
    }

    public void SetupSword(Vector2 dir, float gravityScale)
    {
        _rb.velocity = dir;
        _rb.gravityScale = gravityScale;
    }
    
}
