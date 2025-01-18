using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;
    [SerializeField] private List<UI_CraftSlot> craftSlots;
    
    //private bool _isSetup = false;
    
    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftSlot();
        SetupDefaultCraftWindow();
    }

    private void SetupCraftSlot()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
        
        //_isSetup = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if(_isSetup == false)
            SetupCraftSlot();
    }

    public void SetupDefaultCraftWindow()
    {
        if(craftEquipment[0] != null)
                GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }
    
}
