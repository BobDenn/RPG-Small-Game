using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystall_Skill_Controller : MonoBehaviour
{
    // advance write type
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            CrystalExplode();
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().WasDamaged();
        }
    }

    public void CrystalExplode()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
