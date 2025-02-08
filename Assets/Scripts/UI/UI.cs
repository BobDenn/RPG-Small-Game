using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class UI : MonoBehaviour
{
    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject skillsUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    
    public UI_ItemInfoTip itemInfoTip;
    public UI_StatInfoTip statInfoTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillInfoTip skillInfoTip;

    private void Awake()
    {
        
        SwitchTo(skillsUI);
        
        fadeScreen.gameObject.SetActive(true);
    }

    public void Start()
    {
        SwitchTo(inGameUI);
        
        itemInfoTip.gameObject.SetActive(false);
        //statInfoTip.gameObject.SetActive(false)
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKeyTo(optionsUI);
        
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);
        
        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillsUI);
        
        if (Input.GetKeyDown(KeyCode.P))
            SwitchWithKeyTo(craftUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        
        for (int i = 0; i < transform.childCount; i++)
        {
            // keep the fadeScreen object active
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            
            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        
        if(_menu != null)
            _menu.SetActive(true);
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        
        SwitchTo(menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }
        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeIn();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartGame();
}
