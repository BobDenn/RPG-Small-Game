using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item Effect/Freeze Effect")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.ItemFreezeTime(duration);
            
                
            
        }
    }
}
