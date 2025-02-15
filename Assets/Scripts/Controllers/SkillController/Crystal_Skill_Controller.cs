using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crystal_Skill_Controller : MonoBehaviour
{
    // advance write type
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;
    
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy; 
    // don't forget go to unity and change the layer

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }
    public void ChooseRandomEnemy()
    {
        // get correct radius
        float radius = SkillManager.instance.blackHole.getBlackHoleRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            CrystalExplode();
        }
        // crystal moving
        if (canMove)
        {
            if (closestTarget == null)
                return;
            
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                CrystalExplode();
                canMove = false;
            }
            
            if(closestTarget == null)
                return;
        }
        // become big one
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                //hit.GetComponent<Enemy>().WasDamaged();

                ItemData_Equipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if(equippedAmulet != null)
                    equippedAmulet.Effect(hit.transform);
            }
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
