using UnityEngine;
using System;
using System.Collections;

public enum StatsType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    Health,
    Armor,
    Evasion,
    MagicRes,
    FireDamage,
    IceDamage,
    LightingDamage
}

public class CharacterStats : MonoBehaviour
{
    #region Variables
    private EntityFX _fx;

    [Header("Major status")]
    public Stats vitality; // 生命力 每 1点 增加 10 hp
    public Stats strength; // 力量 每 1点增加 1点 damage和 1%的 critPower
    public Stats agility;  // 敏捷 每 1点增加 1%的 evasion和 critChance
    public Stats intelligence; // 智力 每 1点增加 1点魔法伤害和 1点魔法抗性

    [Header("Offensive status")]
    public Stats damage;    // 基础伤害
    public Stats critChance;// 暴击率
    public Stats critPower; // 暴击伤害 default value 150%


    [Header("Defensive status")]
    public Stats maxHp;   //生命值
    public Stats armour;  //护甲
    public Stats evasion; //闪避
    public Stats magicResistance; // 魔法抗性

    [Header("Magic status")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;

    [Header("Ailments status")]
    // 负面效果持续时间 4s
    [SerializeField] private float ailmentsDuration = 4;
    public bool isIgnited; //持续伤害
    private float _ignitedTimer;
    private int   _igniteDamage;
    
    private float _igniteDamageTimer;
    private float _igniteDamageCool = .3f;

    public bool isChilled; //减少护甲
    private float _chilledTimer;
    
    public bool isShocked; //减少正确攻击率
    private float _shockedTimer;
    // relate to thunder
    [SerializeField] private GameObject thunderStrikePrefab;
    private int _shockDamage;
    
    public int currentHp;
    public Action OnHpChanged;
    public bool IsDead { get; private set; }
    private bool isVulnerable;
    #endregion
    protected virtual void Start()
    {
        // create default value
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
        if(isIgnited)
            ApplyIgniteDamage();
    }

    public void makeVulnerableFor(float duration) => StartCoroutine(VulnerableForCoroutine(duration));
    
    private IEnumerator VulnerableForCoroutine(float duration)
    {
        isVulnerable = true;
        
        yield return new WaitForSeconds(duration);

        isVulnerable = false;
    }
    
    
    public virtual void IncreaseStatusBy(int _modifer, float _duration, Stats statsToModify)
    {
        // start coroutine for status increase
        StartCoroutine(StatusModCoroutine(_modifer, _duration, statsToModify));
    }

    private IEnumerator StatusModCoroutine(int _modifer, float _duration, Stats statsToModify)
    {
        statsToModify.AddModifier(_modifer);// plus
        yield return new WaitForSeconds(_duration);// update
        statsToModify.RemoveModifier(_modifer);// remove
    }
    
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHp += _amount;

        if(currentHp > GetMaxHpValue())
            currentHp = GetMaxHpValue();

        if(OnHpChanged != null)
        {
            OnHpChanged();
        }
    }

    // 生命值变化
    protected virtual void DecreaseHpBy(int damage)
    {
        // 增加伤害
        if (isVulnerable)
            damage = Mathf.RoundToInt(damage * 1.1f);
        
        currentHp -= damage;

        if(OnHpChanged != null)
            OnHpChanged();
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
            //chill
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
                ThunderStrikeHitNearestTarget();
            }
        }
    }
    
    private void ThunderStrikeHitNearestTarget()
    {
        if(GetComponent<Player>() != null)
            return;

        // find the closest enemy, only among enemies
        // instantiate thunder strike
        // setup thunder strike
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        // to find the closest enemy
        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null) && Vector2.Distance(transform.position, hit.transform.position)>1)
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
            GameObject newShockStrike = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<Thunder_Controller>().Setup(_shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    #endregion
    
    #region damage calculate

    public virtual void TakeDamage(int damage)
    {
        DecreaseHpBy(damage);
        
        

        GetComponent<Entity>().WasDamaged();
        _fx.StartCoroutine("FlashFX");
        //Debug.Log(damage);

        if (currentHp <= 0 && !IsDead)
            Die();
    }

    // [damage] 攻击别人
    public virtual void DoDamage(CharacterStats targetStats)
    {
        if(TargetCanAvoidAttack(targetStats))
            return;
        
        targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);
        
        // 总伤害
        int totalDamage = damage.GetValue() + strength.GetValue();
        if(CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);

            //Debug.Log("Total Crit Damage is " + totalDamage);
        }
        // 护甲衰减
        totalDamage = CheckTargetArmour(targetStats, totalDamage);
        targetStats.TakeDamage(totalDamage);
        //Debug.Log(totalDamage);
        
        //if you want you can enable this or if inventory current weapon has fire effect
        DoMagicalDamage(targetStats);// remove if you don't want to apply magic hit on primary attack
    }

    #endregion
    
    #region magical damage
    public void SetupIgniteDamage(int _damage) => _igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => _shockDamage = _damage;
    
    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;
            
        _shockedTimer = ailmentsDuration;
        isShocked = _shock;

        _fx.ShockFxFor(ailmentsDuration);
    }
    private void ApplyIgniteDamage()
    {
        if(_igniteDamageTimer < 0)
        {
            //Debug.Log("Take burn damage" + _igniteDamage);
            
            DecreaseHpBy(_igniteDamage);
            if(currentHp < 0 && !IsDead)
                Die();

            _igniteDamageTimer = _igniteDamageCool;
        }
    }

    public virtual void DoMagicalDamage(CharacterStats targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetMagicResistance(targetStats, totalMagicDamage);

        targetStats.TakeDamage(totalMagicDamage);
        //Debug.Log("totalMagicDamage is" + totalMagicDamage);//25+26+27-20 = 58
        // 异常处理
        if(Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        AttemptyToApplyAilments(targetStats, _fireDamage, _iceDamage, _lightningDamage);
        
    }

    private void AttemptyToApplyAilments(CharacterStats targetStats, int _fireDamage, int _iceDamage,
        int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill  = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock  = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;
        //  增加随机性  如果有相等的伤害值 则进入下面循环
        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(UnityEngine.Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if(UnityEngine.Random.value < 0.3f && _iceDamage > 0)
            {
                canApplyChill = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if(UnityEngine.Random.value < 0.3f && _lightningDamage > 0)
            {
                canApplyShock = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); 
                return;
            }
        }
        if(canApplyIgnite)
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if(canApplyShock)
            targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .1f));

        targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    
    #endregion
    
    #region crit damage
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }
    // 暴击伤害
    protected int CalculateCriticalDamage(int _damage)
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

    public virtual void OnEvasion()
    {
        
    }
    
    protected bool TargetCanAvoidAttack(CharacterStats targetStats)
    {   // 满值 100
        int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();

        if(isShocked)
            totalEvasion += 20;
        
        // 闪避
        // .Net 和 Unity中都有Random这个方法，冲突时要加以前缀区分
        if(UnityEngine.Random.Range(0, 101) < totalEvasion)
        {
            targetStats.OnEvasion();    
            return true;
            //Debug.Log("ATTACK AVOIDED")
        }
        return false;
    }

    protected int CheckTargetArmour(CharacterStats targetStats, int totalDamage)
    {

        if(targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(targetStats.armour.GetValue() * 0.8f);
        else
            totalDamage -= targetStats.armour.GetValue();

        // 护甲会使总伤害衰减，直接减护甲值 不是百分比
        // 防止护甲为负数，以免总伤会出现治疗目标的情况
        totalDamage = Math.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetMagicResistance(CharacterStats targetStats, int totalMagicDamage)
    {
        // 由智力提供的魔抗是 1倍
        totalMagicDamage -= targetStats.magicResistance.GetValue() + (targetStats.intelligence.GetValue());
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    #endregion
    protected virtual void Die()
    {
        IsDead = true;
        //throw new System.NotImplementedException();
    }

    public void KillEntity()
    { 
        if(!IsDead)
            Die();
    }
    
    // calculate health value
    public int GetMaxHpValue()
    {
        return maxHp.GetValue() + vitality.GetValue() * 10;
    }

    public Stats GetStats(StatsType statsType)
    {
        if (statsType == StatsType.Strength) return strength;
        else if (statsType == StatsType.Agility) return agility;
        else if (statsType == StatsType.Intelligence) return intelligence;
        else if (statsType == StatsType.Vitality) return vitality;
        else if (statsType == StatsType.Damage) return damage;
        else if (statsType == StatsType.CritChance) return critChance;
        else if (statsType == StatsType.CritPower) return critPower;
        else if (statsType == StatsType.Health) return maxHp;
        else if (statsType == StatsType.Armor) return armour;
        else if (statsType == StatsType.Evasion) return evasion;
        else if (statsType == StatsType.MagicRes) return magicResistance;
        else if (statsType == StatsType.Evasion) return evasion;
        else if (statsType == StatsType.FireDamage) return fireDamage;
        else if (statsType == StatsType.IceDamage) return iceDamage;
        else if (statsType == StatsType.LightingDamage) return lightningDamage;
        
        return null;
    }
}