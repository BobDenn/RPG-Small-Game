using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        // player stats
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // how much to heal
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHpValue() * healPercent);
        // heal
        playerStats.IncreaseHealthBy(healAmount);
    }
}
