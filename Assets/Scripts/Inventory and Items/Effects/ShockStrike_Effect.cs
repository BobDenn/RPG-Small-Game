using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Shock Strike")]
public class ShockStrike : ItemEffect
{
    [FormerlySerializedAs("ShockStrikePrefab")] [SerializeField] private GameObject shockStrikePrefab;

    public override void ExecuteEffect()
    {
        GameObject newShockStrike = Instantiate(shockStrikePrefab);
        
        //TODO: setup new shock strike
    }
}
