using TMPro;
using UnityEngine;

public class UI_ItemInfoTip : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    
    
    public void ShowItemInfo(ItemData_Equipment item)
    {
        if(item == null)
            return;
        
        itemNameText.text = item.itemName;
        itemTypeText.text = item.itemType.ToString();
        itemDescription.text = item.GetDescription();
        
        gameObject.SetActive(true);
        
        AdjustPosition();
    }
    
    public void HideItemInfo()
    {
        gameObject.SetActive(false);
    } 
        
}
