using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    private float _cooldownTimer;

    protected Player player;

    protected void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        // timestamp
        _cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (_cooldownTimer < 0)
        {
            UseSkill();
            // dash cool down
            _cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on cooldown state");
        return false;
    }

    protected virtual void UseSkill()
    {
        // do some skill specific things
    }
    
}
