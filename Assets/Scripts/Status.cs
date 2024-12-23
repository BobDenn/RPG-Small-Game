using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Status
{
    [SerializeField] private int baseValue;

    // modifiers base on initialized value
    public List<int> modifiers;
    
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }
        
        return finalValue;
    }
    // conveniently do some plus or minus things on value, because it's RPG games, there are many items.
    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }
}
