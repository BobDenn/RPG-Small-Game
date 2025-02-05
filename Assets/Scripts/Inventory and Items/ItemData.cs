using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Material")]
public class ItemData : ScriptableObject
{
    // Attribute of item
    public string itemId;
    public Sprite icon;
    public string itemName;
    public ItemType itemType;
    
    [Range(0, 101)]
    public float dropChance;


    protected StringBuilder Sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }
}