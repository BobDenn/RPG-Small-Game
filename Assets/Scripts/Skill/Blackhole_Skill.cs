using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    Blackhole_Skill_Controller _currentBlackHole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void UseSkill()
    {
        base.UseSkill();
        // create black hole 
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        
        _currentBlackHole = newBlackHole.GetComponent<Blackhole_Skill_Controller>();
        
        _currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown, blackHoleDuration);
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!_currentBlackHole)
            return false;
        
        if (_currentBlackHole.playerCanExitState)
        {
            _currentBlackHole = null;
            return true;
        }
        return false;
    }
    
}
