using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Material")]
public class ItemData : ScriptableObject
{
    // type name icon
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    [Range(0, 101)]
    public float dropChance;
}