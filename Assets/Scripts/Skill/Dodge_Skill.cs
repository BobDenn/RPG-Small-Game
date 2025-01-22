using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge Skill")] 
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount = 5;
    private bool dogeUnlocked;
    
    [Header("Mirage Dodge")]
    [SerializeField] private UI_SkillTreeSlot mirageDodgeButton;
    private bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        mirageDodgeButton.GetComponent<Button>().onClick.AddListener(MirageDodge);
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dogeUnlocked = true;
        }
    }

    private void MirageDodge()
    {
        if(mirageDodgeButton.unlocked)
            dogeUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if(dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
