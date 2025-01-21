using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;
    private Image skillImage;
    
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;
    
    public bool unlocked = false;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;


    private void OnValidate()
    {
        gameObject.name = "UI_SkillTreeSlot - " + skillName;
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();
        skillImage.color = lockedColor;
        
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        // 应该解锁
        foreach (var i in shouldBeUnlocked)
        {
            if (!i.unlocked)
            {
                Debug.Log("can unlock skill");
                return;
            }
        }
        // 应该锁着
        foreach (var i in shouldBeLocked)
        {
            if (i.unlocked)
            {
                Debug.Log("this is locked skill");
                return;
            }
        }
        
        unlocked = true;
        
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillInfoTip.ShowSkillInfoTip(skillName, skillDescription);
        
        // need to show on the mouse's position
        /* 
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -150;
        else
            xOffset = 150;
        
        if (mousePosition.y > 320)
            yOffset = -150;
        else
            yOffset = 150;
        
        ui.skillInfoTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);*/

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillInfoTip.HideSkillInfoTip();
    }
}
