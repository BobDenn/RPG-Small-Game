using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    // you can create clone when you start to use dash or dash state is finished
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone Can Duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float rateToDuplicate;
    [Header("Crystal Instead Of  Clone")]
    public bool crystalInsteadOfClone;
    

    public void CreateClone(Transform _clonePosition, Vector3 offset)
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
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform), canDuplicateClone, rateToDuplicate, player);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneDelay(enemyTransform, new Vector3(1 * player.facingDir, 0f, 0f)));

    }

    private IEnumerator CreateCloneDelay(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(.5f);
            CreateClone(transform, offset);
    }
}
