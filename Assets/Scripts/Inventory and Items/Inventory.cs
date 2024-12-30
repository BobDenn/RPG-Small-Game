using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;


    [Header("Inventory UI")] 
    [SerializeField] private Transform allBagItems;

    private UIItemSlot[] itemSlots;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        itemSlots = allBagItems.GetComponentsInChildren<UIItemSlot>();
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

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlots[i].InitSlot(inventoryItems[i]);
        }
    }
    
    public void AddItem(ItemData _item)
    {
        // 物品堆叠
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
        UpdateSlotUI();
    }
    
    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        UpdateSlotUI();
    }
    
}
