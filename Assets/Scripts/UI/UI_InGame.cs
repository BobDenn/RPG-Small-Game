using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;
    private void Start()
    {
        if (playerStats != null)
            playerStats.OnHpChanged += UpdateHpUI;

        skills = SkillManager.instance;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
            SetColdDownOf(dashImage);
        
        if(Input.GetKeyDown(KeyCode.Q))
            SetColdDownOf(parryImage);
        
        if(Input.GetKeyDown(KeyCode.F))
            SetColdDownOf(crystalImage);
        
        if(Input.GetKeyDown(KeyCode.Mouse1))
            SetColdDownOf(swordImage);
        
        if(Input.GetKeyDown(KeyCode.R))
            SetColdDownOf(blackHoleImage);
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
            SetColdDownOf(flaskImage);
        
        CheckColdDownOf(dashImage, skills.dash.cooldown);
        CheckColdDownOf(parryImage, skills.parry.cooldown);
        CheckColdDownOf(crystalImage, skills.crystal.cooldown);
        CheckColdDownOf(swordImage, skills.sword.cooldown);
        CheckColdDownOf(blackHoleImage, skills.blackHole.cooldown);
        CheckColdDownOf(flaskImage, Inventory.instance.flaskCooldown);
    }
    private void UpdateHpUI()
    {
        slider.maxValue =  playerStats.GetMaxHpValue();
        slider.value    = playerStats.currentHp;
    }

    private void SetColdDownOf(Image image)
    {
        if (image.fillAmount <= 0)
            image.fillAmount = 1;
    }

    private void CheckColdDownOf(Image image, float coldDown)
    {
        if(image.fillAmount > 0)
            image.fillAmount -= 1 / coldDown * Time.deltaTime;
    }
    
}
