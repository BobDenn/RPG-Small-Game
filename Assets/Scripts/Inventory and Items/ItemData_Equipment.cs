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
    
    private int _descriptionLength;

    public void Effect(Transform enemyPosition)
    {
        foreach (var item in ItemEffects)
        {
            item.ExecuteEffect(enemyPosition);
        }
    }
    
    
    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.vitality.AddModifier(vitality);
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        
        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);
        
        playerStats.maxHp.AddModifier(maxHp);
        playerStats.armour.AddModifier(armour);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
        
    }

    public void RemoveModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);
        
        playerStats.maxHp.RemoveModifier(maxHp);
        playerStats.armour.RemoveModifier(armour);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public override string GetDescription()
    {
        Sb.Length = 0;
        _descriptionLength = 0;
        // 说明顺序
        AddItemDescription(vitality, "Vitality");
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        
        AddItemDescription(damage, "Damage");
        AddItemDescription(critPower, "Critical Power");
        AddItemDescription(critChance, "Critical Chance");
        
        AddItemDescription(maxHp, "Max HP");
        AddItemDescription(armour, "Armor");
        AddItemDescription(magicResistance, "Magic Resistance");
        AddItemDescription(evasion, "Evasion");
        
        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");

        if (_descriptionLength < 3)
        {
            for (int i = 0; i < _descriptionLength; i++)
            {
                Sb.AppendLine();
                Sb.Append(" ");
            }
        }
        
        
        return Sb.ToString();
    }

    private void AddItemDescription(int value, string name)
    {
        if (value != 0)
        {
            if (Sb.Length > 0)
                Sb.AppendLine();

            if (value > 0)
                Sb.Append("+ "+ value + "  " + name);

            _descriptionLength++;

        }
    }
    
}
