using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        //player.WasDamaged();
        // PlayerManager.instance.WasDamaged();
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        
        GameManager.instance.lostSoulsAmount = PlayerManager.instance.souls;
        PlayerManager.instance.souls = 0;
        
        // 玩家掉落物品
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHpBy(int damage)
    {
        base.DecreaseHpBy(damage);

        if(IsDead)
            return;
        if (damage > GetMaxHpValue() * .3f)
        {
            player.SetupKnockBackPower(new Vector2(10, 6));
            player.fx.ScreenShake(player.fx.highDamageShake);
            // can use some sounds 
        }
        
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if(currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDamage(CharacterStats targetStats, float _multiplier)
    {
        if(TargetCanAvoidAttack(targetStats))
            return;
        
        // 总伤害
        int totalDamage = damage.GetValue() + strength.GetValue();
        
        // 复制体 增伤
        if(_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
            
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
        DoMagicalDamage(targetStats);
    }
}
