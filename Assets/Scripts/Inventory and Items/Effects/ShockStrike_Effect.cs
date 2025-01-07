using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Shock Strike")]
public class ShockStrike : ItemEffect
{
    [SerializeField] private GameObject shockStrikePrefab;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        // shocked when attacking enemy through thunder claw, then destroyed
        GameObject newShockStrike = Instantiate(shockStrikePrefab, enemyPosition.position, Quaternion.identity);
        Destroy(newShockStrike, 1f);
    }
}
