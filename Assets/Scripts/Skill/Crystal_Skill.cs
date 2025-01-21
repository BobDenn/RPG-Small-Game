using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject _currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking Crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


    public override void UseSkill()
    {
        base.UseSkill();

        // use multi crystal skill
        if (CanUseMultiCrystal())
            return;

        // create crystal
        if (_currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            // player exchanges position with crystal 
            Vector2 playerPos = player.transform.position;
            player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPos;
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(_currentCrystal.transform, Vector3.zero);
                Destroy(_currentCrystal);
            }
            else
            {
                // player.transform.position = _currentCrystal.transform.position;
                _currentCrystal.GetComponent<Crystal_Skill_Controller>()?.CrystalExplode();
            }

        }
    }
    public void CreateCrystal()
    {
        _currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = _currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(_currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() => _currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    // section multi crystal
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            // respawn crystal
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                //GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject crystalToSpawn = crystalLeft.Last();
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform),player);
                if (crystalLeft.Count <= 0)
                {
                    // cooldown the skill
                    cooldown = multiStackCooldown;
                    // refill our crystal stacks
                    RefillCrystal();
                }

                return true;
            }
        }

        return false;
    }


    private void RefillCrystal()
    {
        // encourage player to use up their skills
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }

}
