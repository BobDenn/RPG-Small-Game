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
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        // how much to heal
        int healAmount = Mathf.RoundToInt(playerStatus.GetMaxHpValue() * healPercent);
        // heal
        playerStatus.IncreaseHealthBy(healAmount);
    }
}
