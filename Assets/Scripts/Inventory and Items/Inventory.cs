using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour , ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingEquipment;
    
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
    [SerializeField] private Transform statsSlotParent;
    
    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentSlots;
    private UI_StatSlot[] statsSlot;

    [Header("Items cool down")]
    private float lastTimeUseFlask;
    private float lastTimeUseArmor;
    
    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Data Base")] 
    public List<ItemData> ItemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;
    
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
        statsSlot = statsSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
        
    }

    private void AddStartingItems()
    {
        
        foreach (var item in loadedEquipment)
        {
            EquipItem(item);
        }
        
        if (loadedItems.Count > 0)
        {
            foreach (var item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }
        
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if(startingEquipment[i] != null)
                AddItem(startingEquipment[i]);
        }
        
    }

    public void EquipItem(ItemData item)
    {
        // temporary value to store equipment
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

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
        // apply item modifier
        newEquipment.AddModifier();
        
        RemoveItem(item);
        
        UpdateSlotUI();
    }

    public void UnEquipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            // 卸下的装备返回到背包中
            //AddItem(itemToRemove);
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifier();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlots[i].slotType)
                    equipmentSlots[i].InitSlot(item.Value);
            }
        }
        
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
        
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        foreach (var i in statsSlot)
        {
            i.UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if(_item.itemType == ItemType.Equipment && CanAddItem())
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

    public bool CanAddItem()
    {
        if (inventory.Count > inventoryItemSlots.Length)
        {
            Debug.Log("no more space");
            return false;
        }

        return true;
    }
    
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                // add this to used material
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);

        Debug.Log("Here is your item " + _itemToCraft.name);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equippedItem = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _item in equipmentDictionary)
        {
            if (_item.Key.equipmentType == _type)
                equippedItem = _item.Key;
        }

        return equippedItem;
    }

    #region Item Effect
    
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
        if(currentFlask == null)
            return;

        // if it can use - check cool down
        bool canUseFlask = Time.time > lastTimeUseFlask + flaskCooldown;

        if(canUseFlask)
        {
            flaskCooldown = currentFlask.itemCoolDown;
            currentFlask.Effect(null);
            // set cool down
            lastTimeUseFlask = Time.time;
        }
        else
            Debug.Log($"Flask on Cool Down");
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > lastTimeUseArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCoolDown;
            lastTimeUseArmor = Time.time;
            return true;
        }
        
        return false;
    }
    #endregion

    public void LoadData(GameData data)
    {

        //GetItemDataBase();
        foreach (var pair in data.inventory)
        {
            foreach (var item in ItemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
        // load equipments
        foreach (var loadedItemId in data.equipmentId)
        {
            foreach (var item in ItemDataBase)
            {
                if (item != null && item.itemId == loadedItemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }

        //Debug.Log("装备加载了");
    }

    public void SaveData(ref GameData data)
    {
        // before saving data we need to clear data
        data.inventory.Clear();
        data.equipmentId.Clear();

        foreach (var pair in inventoryDictionary)
        {
            data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (var pair in stashDictionary)
        {
            data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (var pair in equipmentDictionary)
        {
            data.equipmentId.Add(pair.Key.itemId);
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => ItemDataBase = new List<ItemData>(GetItemDataBase());
    
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        String[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });// contains materials and equipments

        foreach (var SOname in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOname);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
#endif    
}
