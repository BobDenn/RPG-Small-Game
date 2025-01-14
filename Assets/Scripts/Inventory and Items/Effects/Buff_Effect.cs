using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats _stats;

    [SerializeField] private StatsType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        _stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        _stats.IncreaseStatusBy(buffAmount, buffDuration, _stats.GetStats(buffType));
    }

    
}
