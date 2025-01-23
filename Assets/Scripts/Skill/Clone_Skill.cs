using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")] 
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    private float _attackMultiplier;

    [Space]
    // you can create clone when you start to use dash or dash state is finished
    /*[SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;*/
    //[SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool cloneCanAttack;

    [Space] [Header("Aggressive clone")] 
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect;
    
    [Header("Clone Can Duplicate")]
    [SerializeField] private UI_SkillTreeSlot duplicateUnlockButton;
    [SerializeField] private float duplicateAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float rateToDuplicate;
    
    [Header("Crystal Instead Of  Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        duplicateUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDuplicateClone);
        crystalInsteadUnlockButton.gameObject.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }


    #region Unlock Skills

    // clone attack
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            cloneCanAttack = true;
            _attackMultiplier = cloneAttackMultiplier;
        }
    }
    // aggressive clone
    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            _attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }
    //duplicate clone
    private void UnlockDuplicateClone()
    {
        if (duplicateUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            _attackMultiplier = duplicateAttackMultiplier;
        }
    }
    //crystal instead clone
    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
            crystalInsteadOfClone = true;
    }
    

    #endregion

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        // crystal can instead of clone
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        // I don't know how to use Instantiate
        GameObject newClone = Instantiate(clonePrefab);

        // clone's position
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration, cloneCanAttack, offset, FindClosestEnemy(newClone.transform), canDuplicateClone, rateToDuplicate, player, cloneAttackMultiplier);
    }
    
    public void CreateCloneWithDelay(Transform enemyTransform)
    {
        //if (canCreateCloneOnCounterAttack)
            StartCoroutine(CloneDelayCoroutine(enemyTransform, new Vector3(1 * player.facingDir, 0f, 0f)));

    }

    private IEnumerator CloneDelayCoroutine(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(.5f);
            CreateClone(transform, offset);
    }
}
