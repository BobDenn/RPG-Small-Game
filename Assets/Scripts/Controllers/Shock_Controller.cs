using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats target = other.GetComponent<EnemyStats>();
            
            playerStats.DoMagicalDamage(target);
        }
    }
}