using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data ", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

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
    

    public void AddModifier()
    {
        
    }

    public void RemoveModifier()
    {
        
    }
}
