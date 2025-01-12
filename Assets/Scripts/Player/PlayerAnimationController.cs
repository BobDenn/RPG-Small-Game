using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Start to attack
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        // detect enemies whom in circle and attack them
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // calculate value
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                
                if(_target != null)
                    player.stats.DoDamage(_target);
                
                //inventory get weapon call item effect
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
                
                // hit.GetComponent<Enemy>().WasDamaged();
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }

}
