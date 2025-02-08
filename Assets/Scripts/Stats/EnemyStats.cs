using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stats soulsDropAmount;

    [Header("Level details")] 
    [SerializeField] private int level = 1;

    [Range(0f, 1f)] [SerializeField] private float percentageModifier = .2f;
    
    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        
        ApplyLevelModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }
    
    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        
        Modify(damage);
        //Modify(critChance);
        //Modify(critPower);
        
        Modify(maxHp);
        Modify(armour);
        Modify(evasion);
        Modify(magicResistance);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
        //souls
        Modify(soulsDropAmount);
    }

    private void Modify(Stats stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stats.GetValue() * percentageModifier;
            
            stats.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        //enemy.WasDamaged();
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();
        PlayerManager.instance.souls += soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();
    }
}
