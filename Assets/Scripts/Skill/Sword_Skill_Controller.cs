using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed;
    private Animator _anim;
    private Rigidbody2D _rb;
    private CircleCollider2D _cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    // need to know what's difference between Awake and Start 
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<CircleCollider2D>();
    }

    // throw sword
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        _rb.velocity = _dir;

        _rb.gravityScale = _gravityScale;
        // siu ~
        _anim.SetBool("Rotate", true);
    }
    // get sword back
    public void ReturnSword()
    {
        _rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
        _anim.SetBool("Rotate", false);
    }

    private void Update()
    {
        if(canRotate)
            transform.right = _rb.velocity;

        if (isReturning) 
        {
            transform.position = Vector2.MoveTowards(
                transform.position, player.transform.position, returnSpeed * Time.deltaTime);
        
            if(Vector2.Distance(transform.position, player.transform.position) < 1)
                player.ClearTheSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        _cd.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
