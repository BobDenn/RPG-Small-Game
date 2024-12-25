using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity _entity;
    private CharacterStatus _myStatus;
    private RectTransform _myTransform;
    private Slider _slider;

    private void Start()
    {
        _myTransform = GetComponent<RectTransform>();
        _entity = GetComponentInParent<Entity>();
        _slider = GetComponentInChildren<Slider>();
        _myStatus = GetComponentInParent<CharacterStatus>();

        _entity.onFlipped += FlipUI;
        _myStatus.onHpChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

// Hp bar is same as HpValue
    private void UpdateHealthUI()
    {
        _slider.maxValue =  _myStatus.GetMaxHpValue();
        _slider.value    = _myStatus.currentHp;
    }

    private void FlipUI() => _myTransform.Rotate(0, 180, 0);
        //Debug.Log("Entity is flipped");

    private void OnDisable() 
    {
        _entity.onFlipped -= FlipUI;
        _myStatus.onHpChanged -= UpdateHealthUI;

    }
    
    
}
