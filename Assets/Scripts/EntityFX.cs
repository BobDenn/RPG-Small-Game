using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Player _player;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject _popUpText;
    
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
    
    [Header("Flash FX")] 
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material _originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    [Header(" Ailment Particles")] 
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;
    
    [Header("Hit fx")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;

    [Space] [SerializeField] private ParticleSystem dustFx;
    
    private void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _player = PlayerManager.instance.player;
        cMIS = GetComponent<CinemachineImpulseSource>();
        _originalMat = _sr.material;
        
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreatePopUpText(string text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1.5f, 3);
        
        Vector3 pos = new Vector3(randomX, randomY, 0);
        GameObject newPopUpText = Instantiate(afterImagePrefab, transform.position + pos, Quaternion.identity);
        newPopUpText.GetComponent<TextMeshPro>().text = text;
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
        
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ChillFxFor(float _seconds)
    {
        chillFx.Play();
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();
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

    public void CreateHitFx(Transform target, bool critical)
    {
        
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);
        
        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFX;

        if (critical)
        {
            hitPrefab = criticalHitFX;
            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);

        }
        
        GameObject newHitFx = Instantiate(hitPrefab, target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        
        newHitFx.transform.Rotate(hitFxRotation);  
        
        Destroy(newHitFx, .5f);
    }

    public void PlayDustFX()
    {
        if(dustFx != null)
            dustFx.Play();
    }
    
}
