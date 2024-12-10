using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;
    
    [Header("Bounce info")] 
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;

    [Header("Pierce info")] 
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float pierceGravity;
    
    [Header("Sword info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    // where sword go, destination
    private Vector2 _finalDir;
    
    [Header("Aim info")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] _dots;

    protected override void Start()
    {
        base.Start();
        
        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
    }

    //direction and multiply our launchDir setting for distance
    protected override void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
            _finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    
    // generate sword
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        // this is one of Swords which can bounce, there is four kinds of sword
        if (swordType == SwordType.Bounce) 
            // add pierce situation
            newSwordScript.SetupBounce(true, amountOfBounce);
        else if(swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(amountOfPierce);
        
        
        // assign the value
        newSwordScript.SetupSword(_finalDir, swordGravity, player);
        player.AssignNewSword(newSword);
        
        // close dots
        DotsActive(false);
    }
    
    #region Aim region
    private Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        _dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            _dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            _dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float a)
    {
        Vector2 position = (Vector2) player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x, 
            AimDirection().normalized.y * launchForce.y) * a + 
                           .5f * (Physics2D.gravity * swordGravity) * (a * a);
        
        return position;
    }
    #endregion
}
