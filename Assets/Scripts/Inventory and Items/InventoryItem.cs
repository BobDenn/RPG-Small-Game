using System;

[Serializable]

public class InventoryItem // Item data and a mount of it
{
    //  ItemData = name + icon
    public ItemData data;

    public int stackSize;
    // InventoryItem = name + icon + stackSize
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
