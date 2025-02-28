using System;
using UnityEngine;
using Cinemachine;

public class PlayerFX : EntityFX
{
    [Header("Screen shake FX")] 
    private CinemachineImpulseSource cMIS;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakePower;
    public Vector3 highDamageShake;
    
    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;   
    [SerializeField] private float colorLoseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    protected void Start()
    {
        base.Start();
        cMIS = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }
    
    public void ScreenShake(Vector3 swordPower)
    {
        cMIS.m_DefaultVelocity = new Vector3(swordPower.x * _player.facingDir, swordPower.y) * shakeMultiplier;
        cMIS.GenerateImpulse();
    }
    

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFx>().SetupAfterImageFx(colorLoseRate, _sr.sprite);
        }
    }
}
