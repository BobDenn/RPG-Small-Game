using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
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

        player.WasDamaged();
        // PlayerManager.instance.WasDamaged();
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
