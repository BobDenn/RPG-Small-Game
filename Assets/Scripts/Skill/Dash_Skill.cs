using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash Skill")] 
    public bool dashUnlocked;
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    
    [Header("Clone on Dash")] 
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    
    [Header("Clone on arrival")] 
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    
    
    
    protected override void UseSkill()
    {
        base.UseSkill();
        
        //Debug.Log("Created clone behind");
    }

    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);        
    }

    private void UnlockDash()
    {
        // can't hide SkillTree_UI, otherwise there are errors that cant turn "Dash Unlocked" to be ture.
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    
    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
    public void CreateCloneOnDashOver()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

}
