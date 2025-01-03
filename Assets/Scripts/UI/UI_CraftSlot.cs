using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        InitSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // inventory craft item data
        
        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
