using System.Collections.Generic;
using UnityEngine.Serialization;
using System.Collections;
using UnityEngine;
using System;


/// <summary>
/// basic value of character and attack activities
/// </summary>
public class CharacterStatus : MonoBehaviour
{
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
    public bool isIgnited; //持续伤害
    private float _ignitedTimer;
    private int   _igniteDamage;
    private float _igniteDamageTimer;
    private float _igniteDamageCool = .3f;

    public bool isChilled; //减少护甲
    private float _chilledTimer;
    
    public bool isShocked; //减少正确攻击率
    private float _shockedTimer;


    public int currentHp;
    public System.Action onHpChanged;

    
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = GetMaxHpValue();
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
    // 施加负面效果
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if(isIgnited || isChilled || isShocked)
            return;

        if(_ignite)
        {
            isIgnited = _ignite;
            _ignitedTimer = 2;
        }

        if(_chill)
        {
            isChilled = _chill;
            _chilledTimer = 2;
        }

        if(_shock)
        {
            isShocked = _shock;
            _shockedTimer = 2;
        }
    }
#region damage calculate

    public void SetupIgniteDamage(int _damage) => _igniteDamage = _damage;

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;

        //Debug.Log(damage);

        if (currentHp <= 0)
            Die();
    }

    protected virtual void DecreaseHpBy(int _damage)
    {
        DecreaseHpBy(_damage);

        if(onHpChanged != null)
            onHpChanged();
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
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

}