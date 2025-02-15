using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;

    private float cloneTimer;
    private float attackMultiplier;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float chanceToDuplicate;

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
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack,Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate,Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNum", Random.Range(1, 4)); // range( [x,y) )

        _attackMultiplier = attackMultiplier;
        player = _player;

        transform.position = _newTransform.position + _offset;

        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;

        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        
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
            {
                // player damages
                //hit.GetComponent<Enemy>().WasDamaged();
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                
                playerStats.CloneDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }
                
                if(canDuplicateClone)
                {
                    // generate clone rate
                    if(Random.Range(1, 101) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1.1f * facingDir, 0, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        // if clone on Enemy right side, then face to target
        if (closestEnemy != null) 
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
