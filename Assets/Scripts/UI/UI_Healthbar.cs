using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    private Entity _entity;
    private CharacterStats _myStats;
    private RectTransform _myTransform;
    private Slider _slider;

    private void Start()
    {
        _myTransform = GetComponent<RectTransform>();
        _entity = GetComponentInParent<Entity>();
        _slider = GetComponentInChildren<Slider>();
        _myStats = GetComponentInParent<CharacterStats>();

        _entity.OnFlipped += FlipUI;
        _myStats.OnHpChanged += UpdateHpUI;

        UpdateHpUI();
    }

    /*private void Update()
    {
        UpdateHpUI();
    }*/

// Hp bar is same as HpValue
    private void UpdateHpUI()
    {
        _slider.maxValue =  _myStats.GetMaxHpValue();
        _slider.value    = _myStats.currentHp;
    }

    private void FlipUI() => _myTransform.Rotate(0, 180, 0);
        //Debug.Log("Entity is flipped");

    private void OnDisable() 
    {
        _entity.OnFlipped -= FlipUI;
        _myStats.OnHpChanged -= UpdateHpUI;
    }
    
    
}
