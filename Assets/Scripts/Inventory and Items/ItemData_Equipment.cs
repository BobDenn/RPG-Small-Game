using System.Collections.Generic;
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
    public float itemCoolDown;
    public EquipmentType equipmentType;

    // make special items have some unique effections
    public ItemEffect[] ItemEffects;
    
    

    [Header("Major status")]
    public int vitality; // 生命力
    public int strength; // 力量 
    public int agility;  // 敏捷
    public int intelligence; // 智力 

    [Header("Offensive status")]
    public int damage;    // 基础伤害
    public int critChance;// 暴击率
    public int critPower; // 暴击伤害


    [Header("Defensive status")]
    public int maxHp;   //生命值
    public int armour;  //护甲
    public int evasion; //闪避
    public int magicResistance; // 魔法抗性

    [Header("Magic status")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void Effect(Transform enemyPosition)
    {
        foreach (var item in ItemEffects)
        {
            item.ExecuteEffect(enemyPosition);
        }
    }
    
    
    public void AddModifier()
    {
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        
        playerStatus.vitality.AddModifier(vitality);
        playerStatus.strength.AddModifier(strength);
        playerStatus.agility.AddModifier(agility);
        playerStatus.intelligence.AddModifier(intelligence);
        
        playerStatus.damage.AddModifier(damage);
        playerStatus.critChance.AddModifier(critChance);
        playerStatus.critPower.AddModifier(critPower);
        
        playerStatus.maxHp.AddModifier(maxHp);
        playerStatus.armour.AddModifier(armour);
        playerStatus.evasion.AddModifier(evasion);
        playerStatus.magicResistance.AddModifier(magicResistance);
        
        playerStatus.fireDamage.AddModifier(fireDamage);
        playerStatus.iceDamage.AddModifier(iceDamage);
        playerStatus.lightningDamage.AddModifier(lightningDamage);
        
    }

    public void RemoveModifier()
    {
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        
        playerStatus.vitality.RemoveModifier(vitality);
        playerStatus.strength.RemoveModifier(strength);
        playerStatus.agility.RemoveModifier(agility);
        playerStatus.intelligence.RemoveModifier(intelligence);
        
        playerStatus.damage.RemoveModifier(damage);
        playerStatus.critChance.RemoveModifier(critChance);
        playerStatus.critPower.RemoveModifier(critPower);
        
        playerStatus.maxHp.RemoveModifier(maxHp);
        playerStatus.armour.RemoveModifier(armour);
        playerStatus.evasion.RemoveModifier(evasion);
        playerStatus.magicResistance.RemoveModifier(magicResistance);
        
        playerStatus.fireDamage.RemoveModifier(fireDamage);
        playerStatus.iceDamage.RemoveModifier(iceDamage);
        playerStatus.lightningDamage.RemoveModifier(lightningDamage);
    }
}
