using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    
    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentSlots;// equipment statement
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        stash = new List<InventoryItem>();
        inventory = new List<InventoryItem>();
        equipment = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // 获取栈顶
            ItemData newItem = inventoryItems.Last().data;
            // 然后移除
            RemoveItem(newItem);
        }
    }*/

    // click equipment and wear it
    public void EquipItem(ItemData item)
    {
        // temporary value 
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _item in equipmentDictionary)
        {
            if (_item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = _item.Key;
        }

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        RemoveItem(item);
        
        UpdateSlotUI();
    }

    private void UnEquipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value)) // search in bags
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _item in equipmentDictionary)
            {
                // 确保一一对应
                if (_item.Key.equipmentType == equipmentSlots[i].slotType)
                    equipmentSlots[i].InitSlot(_item.Value);
            }
        }
        // use clean up function
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }
        
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].InitSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].InitSlot(stash[i]);
        }
        
    }
    
    public void AddItem(ItemData _item)
    {
        if(_item.itemType == ItemType.Equipment)
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);
            
        
        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }
        
        UpdateSlotUI();
    }
    
}
