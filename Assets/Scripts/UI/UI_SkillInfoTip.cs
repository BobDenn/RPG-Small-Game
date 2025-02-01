using TMPro;
using UnityEngine;

public class UI_SkillInfoTip : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowSkillInfoTip(string skillName, string skillDescription, int price)
    {
        _skillName.text = skillName;
        skillText.text = skillDescription;
        skillCost.text = "Cost: " + price;
        
        AdjustPosition();
        gameObject.SetActive(true);
    }
    
    public void HideSkillInfoTip() => gameObject.SetActive(false);
}
