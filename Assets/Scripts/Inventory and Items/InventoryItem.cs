using System;


[Serializable]

public class InventoryItem // Item data and a mount of it
{
    //type + name + icon
    public ItemData data;

    public int stackSize;
    // InventoryItem = type + name + icon + stackSize
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
