using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;// IPointerDownHandler

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler // mouseclick interface
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;//data(type + name + icon) + stackSize
    protected UI UI;

    protected virtual void Start()
    {
        UI = GetComponentInParent<UI>();
    }

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
        if (item == null)
            return;
        
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        
        
        // only equip equipment
        if(item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
        
        UI.itemInfoTip.HideItemInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        
        UI.itemInfoTip.ShowItemInfo(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null)
            return;
        
        UI.itemInfoTip.HideItemInfo();
    }
}
