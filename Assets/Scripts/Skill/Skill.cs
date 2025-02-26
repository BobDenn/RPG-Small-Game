using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        LoadedSkillCheck();
    }

    protected virtual void Update()
    {
        // timestamp
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void LoadedSkillCheck()
    {
        
    }
    
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            // dash cool down
            cooldownTimer = cooldown;
            return true;
        }

        player.fx.CreatePopUpText("Cooldown...");
        return false;
    }

    public virtual void UseSkill()
    {
        // do some skill specific things
    }
    
    // find the closest enemy
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        // detect enemy within r=25's Circle 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        // to find the closest enemy
        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null))
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    // update closestDistance when detect enemy successfully
                    closestDistance = distanceToEnemy;
                    // got closestEnemy
                    closestEnemy = hit.transform;
                }
            }
        }
        
        return closestEnemy;
    }
    
}
