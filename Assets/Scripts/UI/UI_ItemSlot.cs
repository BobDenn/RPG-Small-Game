using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;// IPointerDownHandler

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler // mouseclick interface
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;//data(type + name + icon) + stackSize

    public void InitSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;
        
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // only equip equipment
        if(item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }
}
