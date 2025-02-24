using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLoseRate;

    public void SetupAfterImageFx(float losingSpeed, Sprite spriteImg)
    {
        sr = GetComponent<SpriteRenderer>();
        
        sr.sprite = spriteImg;
        colorLoseRate = losingSpeed;
    }

    private void Update()
    {
        float alpha = sr.color.a - colorLoseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        
        if(sr.color.a <= 0)
            Destroy(gameObject);
    }
}
