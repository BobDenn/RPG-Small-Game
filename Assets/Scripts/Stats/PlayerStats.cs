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
        
        // 玩家掉落物品
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHpBy(int damage)
    {
        base.DecreaseHpBy(damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if(currentArmor != null)
            currentArmor.Effect(player.transform);
        
    }
}
