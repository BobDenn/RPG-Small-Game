using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment data)
    {
        if (data == null)
            return;
        
        item.data = data;
        itemImage.sprite = data.icon;
        itemText.text = data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // inventory craft item data
        
        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
