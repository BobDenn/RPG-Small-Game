using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// basic value of character
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    public Status damage;
    public Status maxHp;
    
    
    [SerializeField] private int currentHp;
    
    void Start()
    {
        currentHp = maxHp.getValue();
        
        // example equip sword with 4 damage
        damage.AddModifier(4);
    }
    

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp < 0)
            Die();
    }

    private void Die()
    {
        throw new System.NotImplementedException();
    }
}
