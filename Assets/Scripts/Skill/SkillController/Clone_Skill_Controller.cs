using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    private float cloneTimer;
    private Transform closestEnemy;
    
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    [SerializeField] private float colorLosingSpeed;
    

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        // gradually lose color
        if (cloneTimer < 0) 
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

        // destroy extra clone
        if(sr.color.a <= 0)
            Destroy(gameObject);

    }

    // create a clone of player such as this function's name
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack,Vector3 _offset)
    {
        if (_canAttack)
            anim.SetInteger("AttackNum", Random.Range(1, 4)); // range( [x,y) )

        transform.position = _newTransform.position + _offset;

        cloneTimer = _cloneDuration;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        // detect enemies whom in circle and attack them
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().WasDamaged();
        }
    }

    private void FaceClosestTarget()
    {
        // detect enemy within r=25's Circle 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        // to find the closest enemy
        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    // update closestDistance when detect enemy successfully
                    closestDistance = distanceToEnemy;
                    // got closestEnemy
                    closestEnemy = hit.transform;
                }
            }
        }

        // if clone on Enemy right side, then face to target
        if (closestEnemy != null) 
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
