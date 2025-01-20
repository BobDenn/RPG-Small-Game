using TMPro;
using UnityEngine;

public class UI_SkillInfoTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI skillText;

    public void ShowSkillInfoTip(string skillName, string skillDescription)
    {
        _skillName.text = skillName;
        skillText.text = skillDescription;
        gameObject.SetActive(true);
    }
    
    public void HideSkillInfoTip() => gameObject.SetActive(false);
}
