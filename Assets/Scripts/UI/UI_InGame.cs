using TMPro;
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
    
    // record souls
    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private int increaseRate = 100;
    
    private void Start()
    {
        if (playerStats != null)
            playerStats.OnHpChanged += UpdateHpUI;

        skills = SkillManager.instance;
    }

    private void Update()
    {
        UpdateSoulsUI();

        if(Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetColdDownOf(dashImage);
        
        if(Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetColdDownOf(parryImage);
        
        if(Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetColdDownOf(crystalImage);
        
        if(Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetColdDownOf(swordImage);
        
        if(Input.GetKeyDown(KeyCode.R) && skills.blackHole.blackHoleUnlocked)
            SetColdDownOf(blackHoleImage);
        
        if(Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetColdDownOf(flaskImage);
        
        CheckColdDownOf(dashImage, skills.dash.cooldown);
        CheckColdDownOf(parryImage, skills.parry.cooldown);
        CheckColdDownOf(crystalImage, skills.crystal.cooldown);
        CheckColdDownOf(swordImage, skills.sword.cooldown);
        CheckColdDownOf(blackHoleImage, skills.blackHole.cooldown);
        CheckColdDownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if(soulsAmount < PlayerManager.instance.GetSouls())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetSouls();

        currentSouls.text = ((int)soulsAmount).ToString("N0");
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
