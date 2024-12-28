using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _sr;

    [Header("Flash FX")] 
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material _originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _sr.material;
        
    }
    // disappear
    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            _sr.color = Color.clear;
        else
            _sr.color = Color.white;
    }
    
    // hit flash conflict with ailment color(solved)
    private IEnumerator FlashFX()
    {
        _sr.material = hitMat;
        Color currentColor = _sr.color;
        _sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        _sr.color = currentColor;
        _sr.material = _originalMat;
    }

    // to change _sr's color
    private void RedColorBlink()
    {
        if(_sr.color != Color.white)
            _sr.color = Color.white;
        else
            _sr.color = Color.red;
    }
    
    // cancel repeating
    private void CancelColorChange()
    {
        // stop repeating
        CancelInvoke();
        _sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFX()
    {
        if(_sr.color != igniteColor[0])
            _sr.color = igniteColor[0];
        else
            _sr.color = igniteColor[1];
    }

    private void ChillColorFX()
    {
        if(_sr.color != chillColor[0])
            _sr.color = chillColor[0];
        else
            _sr.color = chillColor[1];
    }

    private void ShockColorFX()
    {
        if(_sr.color != shockColor[0])
            _sr.color = shockColor[0];
        else
            _sr.color = shockColor[1];
    }
}
