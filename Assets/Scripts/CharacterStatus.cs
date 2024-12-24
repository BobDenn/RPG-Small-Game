using System.Collections.Generic;
using UnityEngine.Serialization;
using System.Collections;
using UnityEngine;
using System;


/// <summary>
/// basic value of character
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

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    [SerializeField] private int currentHp;
    
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = maxHp.GetValue();
        
    }
    // [damage] 
    public virtual void DoDamage(CharacterStatus _targetStatus)
    {
        if(TargetCanAvoidAttack(_targetStatus))
            return;
        
        // 总伤害
        int totalDamage = damage.GetValue() + strength.GetValue();
        if(CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            Debug.Log("Total Crit Damage is " + totalDamage);
        }
        // 护甲衰减
        totalDamage = CheckTargetArmour(_targetStatus, totalDamage);

        //_targetStatus.TakeDamage(totalDamage);
        //Debug.Log(totalDamage);

        DoMagicDamage(_targetStatus);
    }

    public virtual void DoMagicDamage(CharacterStatus _targetStatus)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        // 由智力提供的魔抗是3倍
        totalMagicDamage = CheckTargetMagicResistance(_targetStatus, totalMagicDamage);

        _targetStatus.TakeDamage(totalMagicDamage);
    }

    private static int CheckTargetMagicResistance(CharacterStatus _targetStatus, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStatus.magicResistance.GetValue() + (_targetStatus.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if(isIgnited || isChilled || isShocked)
            return;

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;

        //Debug.Log(damage);

        if (currentHp <= 0)
            Die();
    }

    protected virtual void Die()
    {
        //throw new System.NotImplementedException();
    }

    private bool TargetCanAvoidAttack(CharacterStatus _targetStatus)
    {
        int totalEvasion = _targetStatus.evasion.GetValue() + _targetStatus.agility.GetValue();
        
        // 闪避
        // .Net 和 Unity中都有Random这个方法，冲突时要加以前缀区分
        if(UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
            //Debug.Log("ATTACK AVOIDED")
        }
        return false;
    }

    private  int CheckTargetArmour(CharacterStatus _targetStatus, int totalDamage)
    {
        // 护甲会使总伤害衰减
        totalDamage -= _targetStatus.armour.GetValue();
        // 防止护甲为负数，以免总伤会出现治疗目标的情况
        totalDamage = Math.Clamp(totalDamage, 0, int.MaxValue);

        return totalDamage;
    }

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
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        //Debug.Log("Total crit power is " + totalCritPower*100+ "%");

        float critDamage = _damage * totalCritPower;
        //Debug.Log("crit damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }

}
