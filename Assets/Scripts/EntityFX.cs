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

    private void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _sr.material;
        
        

    }

    // hit flash
    private IEnumerator FlashFX()
    {
        _sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

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
    private void CancelRebBlink()
    {
        // stop repeating
        CancelInvoke();
        _sr.color = Color.white;
    }
}
