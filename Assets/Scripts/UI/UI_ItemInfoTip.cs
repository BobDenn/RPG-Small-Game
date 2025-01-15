using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemInfoTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;
    
    public void ShowItemInfo(ItemData_Equipment item)
    {
        if(item == null)
            return;
        
        itemNameText.text = item.itemName;
        itemTypeText.text = item.itemType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
            itemNameText.fontSize *= .7f;
        else
            itemNameText.fontSize = defaultFontSize;
        
        gameObject.SetActive(true);
    }
    
    public void HideItemInfo()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    } 
        
}
