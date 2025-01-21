using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry skill")] 
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }
    
    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    public bool restoreUnlocked{ get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float parryRestorePercentage;
    
    [Header("Parry Mirage")]
    [SerializeField] private UI_SkillTreeSlot parryMirageUnlockButton;
    public bool parryMirageUnlocked{ get; private set; }

    
    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHpValue() * parryRestorePercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
            
    }

    protected override void Start()
    {
        base.Start();
        // add events
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);

    }

    private void UnlockParry()
    {
        if(parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    protected void UnlockParryRestore()
    {
        if(restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    protected void UnlockParryMirage()
    {
        if(parryMirageUnlockButton.unlocked)
            parryMirageUnlocked = true;
    }

    public void MakeMirageWhenParry(Transform transform)
    {
        if(parryMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(transform);
    }
    
}
