using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImages;

    public void SetupCraftWindow(ItemData_Equipment data)
    {
        craftButton.onClick.RemoveAllListeners();
        
        foreach (var i in materialImages)
        {
            i.color = Color.clear;
            i.GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < data.craftingMaterials.Count; i++)
        {
            if(data.craftingMaterials.Count > materialImages.Length)
                Debug.LogWarning("you have more materials amount than you have material slots in craft window");

            materialImages[i].sprite = data.craftingMaterials[i].data.icon;
            materialImages[i].color = Color.white;
            
            TextMeshProUGUI materialSlotText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
            
            materialSlotText.text = data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }
        
        itemIcon.sprite = data.icon;
        itemName.text = data.itemName;
        itemDescription.text = data.GetDescription();
        
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(data, data.craftingMaterials));
    }
}
