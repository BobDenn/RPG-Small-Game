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
            PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
            EnemyStatus target = other.GetComponent<EnemyStatus>();
            
            playerStatus.DoMagicalDamage(target);
        }
    }
}
