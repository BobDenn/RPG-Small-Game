using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject _currentCrystal;

    [Header("Explosive crystal")] 
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")] 
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;


    protected override void UseSkill()
    {
        base.UseSkill();

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
}
