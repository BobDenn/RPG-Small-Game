using UnityEngine;
using System;


/// <summary>
/// basic value of character and attack activities
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    private EntityFX _fx;

    [Header("Major status")]
    public Status vitality; // 生命力 每1点 增加3/5 hp
    public Status strength; // 力量 每1点增加1点damage和1%的critPower
    public Status agility;  // 敏捷 每1点增加1%的evasion和critChance
    public Status intelligence; // 智力 每1点增加1点魔法伤害和1点魔法抗性

    [Header("Offensive status")]
    public Status damage;    // 基础伤害
    public Status critChance;// 暴击率
    public Status critPower; // 暴击伤害 default value 150%


    [Header("Defensive status")]
    public Status maxHp;   //生命值
    public Status armour;  //护甲
    public Status evasion; //闪避
    public Status magicResistance; // 魔法抗性

    [Header("Magic status")]
    public Status fireDamage;
    public Status iceDamage;
    public Status lightningDamage;

    [Header("Ailments status")]
    [SerializeField] private float ailmentsDuration = 4;
    public bool isIgnited; //持续伤害
    private float _ignitedTimer;
    private int   _igniteDamage;
    // relate to thunder
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;
    private float _igniteDamageTimer;
    private float _igniteDamageCool = .3f;

    public bool isChilled; //减少护甲
    private float _chilledTimer;
    
    public bool isShocked; //减少正确攻击率
    private float _shockedTimer;


    public int currentHp;
    public Action OnHpChanged;

    
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = GetMaxHpValue();

        _fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        _ignitedTimer -= Time.deltaTime;
        _chilledTimer -= Time.deltaTime;
        _shockedTimer -= Time.deltaTime;

        _igniteDamageTimer -= Time.deltaTime;

        if(_ignitedTimer < 0)
            isIgnited = false;

        if(_chilledTimer < 0)
            isChilled = false;

        if(_shockedTimer < 0)
            isShocked = false;

        if(_igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Take burn damage" + _igniteDamage);


            DecreaseHpBy(_igniteDamage);
            if(currentHp < 0)
                Die();

            _igniteDamageTimer = _igniteDamageCool;
        }
    }
#region Ailments effect
    // 施加负面效果
    private void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill  = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock  = !isIgnited && !isChilled;

        if(ignite && canApplyIgnite)
        {
            isIgnited = ignite;
            _ignitedTimer = ailmentsDuration;

            _fx.IgniteFxFor(ailmentsDuration);
        }

        if(chill && canApplyChill)
        {
            isChilled = chill;
            _chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            _fx.ChillFxFor(ailmentsDuration);
        }
        // shock
        if(shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(shock);
                
            }
            else
            {
                if(GetComponent<Player>() != null)
                    return;

                // find closest enemy, only among enemies
                // instantiate thunder strike
                // setup thunder strike
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

                float closestDistance = Mathf.Infinity;
                Transform closestEnemy = null;

                // to find the closest enemy
                foreach (var hit in colliders)
                {
                    if ((hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position)>1))
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
                    // 如果没有第二个敌人，就打当前敌人
                    if(closestEnemy == null)
                        closestEnemy = transform;

                }

                if(closestEnemy != null)
                {
                    GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

                    newShockStrike.GetComponent<Thunder_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStatus>());
                }

            }
        }
    }
#endregion
#region damage calculate

    public void SetupIgniteDamage(int _damage) => _igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    public virtual void TakeDamage(int damage)
    {
        DecreaseHpBy(damage);

        //Debug.Log(damage);

        if (currentHp <= 0)
            Die();
    }

    protected virtual void DecreaseHpBy(int damage)
    {
        currentHp -= damage;

        if(OnHpChanged != null)
            OnHpChanged();
    }

    // [damage] 攻击别人
    public virtual void DoDamage(CharacterStatus _targetStatus)
    {
        if(TargetCanAvoidAttack(_targetStatus))
            return;
        
        // 总伤害
        int totalDamage = damage.GetValue() + strength.GetValue();
        if(CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);

            //Debug.Log("Total Crit Damage is " + totalDamage);
        }
        // 护甲衰减
        totalDamage = CheckTargetArmour(_targetStatus, totalDamage);
        //_targetStatus.TakeDamage(totalDamage);
        //Debug.Log(totalDamage);
        DoMagicDamage(_targetStatus);
    }

#endregion
#region magic damage
    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;
            
        _shockedTimer = ailmentsDuration;
        isShocked = _shock;

        _fx.ShockFxFor(ailmentsDuration);
    }

    public virtual void DoMagicDamage(CharacterStatus _targetStatus)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetMagicResistance(_targetStatus, totalMagicDamage);

        _targetStatus.TakeDamage(totalMagicDamage);
        //Debug.Log("totalMagicDamage is" + totalMagicDamage);//25+26+27-20 = 58
        // 异常处理
        if(Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill  = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock  = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;
        //  增加随机性  如果有相等的伤害值 则进入下面循环
        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(UnityEngine.Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Apply Ignite");
                return;
            }
            if(UnityEngine.Random.value < 0.3f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Apply Chill");
                return;
            }
            if(UnityEngine.Random.value < 0.3f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Apply Shock");
                return;
            }
        }
        if(canApplyIgnite)
            _targetStatus.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if(canApplyShock)
            _targetStatus.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .1f));

        _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    
#endregion
#region crit damage
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }
    // 暴击伤害
    private int CalculateCriticalDamage(int _damage)
    {
                            //  150 + 5 = 155%
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        //Debug.Log("Total crit power is " + totalCritPower*100+ "%");
                            // 25 * 1.55 = 38.75
        float critDamage = _damage * totalCritPower;
        //Debug.Log("crit damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);// 39
    }
#endregion

#region check defense 
    private bool TargetCanAvoidAttack(CharacterStatus _targetStatus)
    {   // 满值 100
        int totalEvasion = _targetStatus.evasion.GetValue() + _targetStatus.agility.GetValue();

        if(isShocked)
            totalEvasion += 20;
        
        // 闪避
        // .Net 和 Unity中都有Random这个方法，冲突时要加以前缀区分
        if(UnityEngine.Random.Range(0, 101) < totalEvasion)
        {
            return true;
            //Debug.Log("ATTACK AVOIDED")
        }
        return false;
    }

    private  int CheckTargetArmour(CharacterStatus _targetStatus, int totalDamage)
    {

        if(_targetStatus.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStatus.armour.GetValue() * 0.8f);
        else
            totalDamage -= _targetStatus.armour.GetValue();

        // 护甲会使总伤害衰减，直接减护甲值 不是百分比
        // 防止护甲为负数，以免总伤会出现治疗目标的情况
        totalDamage = Math.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private static int CheckTargetMagicResistance(CharacterStatus _targetStatus, int totalMagicDamage)
    {
        // 由智力提供的魔抗是3倍
        totalMagicDamage -= _targetStatus.magicResistance.GetValue() + (_targetStatus.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

#endregion
    protected virtual void Die()
    {
        //throw new System.NotImplementedException();
    }
    // calculate health value
    public int GetMaxHpValue()
    {
        return maxHp.GetValue() + vitality.GetValue() * 5;
    }

}