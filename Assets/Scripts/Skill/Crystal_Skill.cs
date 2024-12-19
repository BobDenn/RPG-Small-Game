using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject _currentCrystal;

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
    [SerializeField] private List<GameObject> crystalleft = new List<GameObject>();


    protected override void UseSkill()
    {
        base.UseSkill();
        
        // use multi crystal skill
        if(CanUseMultiCrystal())
            return;
        
        // create crystal
        if (_currentCrystal == null)
        {
            _currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystall_Skill_Controller currentCrystalScript = _currentCrystal.GetComponent<Crystall_Skill_Controller>();
            
            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(_currentCrystal.transform));
        }
        else
        {
            if(canMoveToEnemy)
                return;
            
            // player exchanges position with crystal 
            Vector2 playerPos = player.transform.position;

            player.transform.position = _currentCrystal.transform.position;

            _currentCrystal.transform.position = playerPos;
            // player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.GetComponent<Crystall_Skill_Controller>()?.CrystalExplode();
        }
    }

    // section multi crystal
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            // respawn crystal
            if (crystalleft.Count > 0)
            {
                if(crystalleft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                
                cooldown = 0;
                //GameObject crystalToSpawn = crystalleft[crystalleft.Count - 1];
                GameObject crystalToSpawn = crystalleft.Last();
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalleft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystall_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalleft.Count <= 0)
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
        int amountToAdd = amountOfStacks - crystalleft.Count;
        
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalleft.Add(crystalPrefab);
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
