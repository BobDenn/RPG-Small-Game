using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackAnimationController : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //hit.GetComponent<Player>().WasDamaged();
                PlayerStatus target = hit.GetComponent<PlayerStatus>();
                enemy.status.DoDamage(target);
            }
        }
    }
    // open and close red thing event
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

}
