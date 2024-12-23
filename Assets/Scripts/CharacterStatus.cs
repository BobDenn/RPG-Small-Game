using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// basic value of character
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    public Status strength;
    public Status damage;
    public Status maxHp;
    
    
    [SerializeField] private int currentHp;
    
    protected virtual void Start()
    {
        currentHp = maxHp.GetValue();
        
        
    }
    // [damage] player to enemy
    public virtual void DoDamage(CharacterStatus _targetStatus)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStatus.TakeDamage(totalDamage);
        Debug.Log(totalDamage);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp < 0)
            Die();
    }

    protected virtual void Die()
    {
        //throw new System.NotImplementedException();
    }
}
