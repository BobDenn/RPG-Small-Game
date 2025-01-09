using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
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

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStatus status;

    [SerializeField] private StatusType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        status = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        
        status.IncreaseStatusBy(buffAmount, buffDuration, StatusToModify());
    }

    private Status StatusToModify()
    {
        if (buffType == StatusType.Strength) return status.strength;
        else if (buffType == StatusType.Agility) return status.agility;
        else if (buffType == StatusType.Intelligence) return status.intelligence;
        else if (buffType == StatusType.Vitality) return status.vitality;
        else if (buffType == StatusType.Damage) return status.damage;
        else if (buffType == StatusType.CritChance) return status.critChance;
        else if (buffType == StatusType.CritPower) return status.critPower;
        else if (buffType == StatusType.Health) return status.maxHp;
        else if (buffType == StatusType.Armor) return status.armour;
        else if (buffType == StatusType.Evasion) return status.evasion;
        else if (buffType == StatusType.MagicRes) return status.magicResistance;
        else if (buffType == StatusType.Evasion) return status.evasion;
        else if (buffType == StatusType.FireDamage) return status.fireDamage;
        else if (buffType == StatusType.IceDamage) return status.iceDamage;
        else if (buffType == StatusType.LightingDamage) return status.lightningDamage;
        
        return null;
    }
}
