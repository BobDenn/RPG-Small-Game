using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    
    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSloParent;
    
    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentSlots;
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
        equipmentSlots = inventorySlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    public void EquipItem(ItemData item)
    {
        // temporary value to store equipment
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(item);

        ItemData_Equipment oldEquipment = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _item in equipmentDictionary)
        {
            if (_item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = _item.Key;
        }
        // replace equipment
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
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
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
                if (_item.Key.equipmentType == equipmentSlots[i].slotType)
                    equipmentSlots[i].InitSlot(_item.Value);
            }
        }
        
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].InitSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].InitSlot(stash[i]);
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
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
