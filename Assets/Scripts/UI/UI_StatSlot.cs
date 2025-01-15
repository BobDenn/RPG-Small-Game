using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI _ui;
    
    [SerializeField] private string statName;
    [SerializeField] private StatsType statsType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;
    
    
    [TextArea]
    [SerializeField] private string statDescription;
    
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        
        if(statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValueUI();
        _ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStats(statsType).GetValue().ToString();
            
            // 正确获取角色数值 并 展示
            if (statsType == StatsType.Health)
                statValueText.text = playerStats.GetMaxHpValue().ToString();
            
            if(statsType == StatsType.Damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            
            if(statsType == StatsType.CritPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            
            if(statsType == StatsType.CritChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if(statsType == StatsType.Evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            
            if(statsType == StatsType.MagicRes)
                statValueText.text = (playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue()).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.statInfoTip.ShowStatInfoTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.statInfoTip.HideStatInfoTip();
    }
}
