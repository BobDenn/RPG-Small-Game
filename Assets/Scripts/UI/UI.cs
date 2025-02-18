using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class UI : MonoBehaviour, ISaveManager
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

    [SerializeField] private UI_VolumeController[] volumeSettings;

    private void Awake()
    {
        
        SwitchTo(skillsUI);
        
        fadeScreen.gameObject.SetActive(true);
        itemInfoTip.gameObject.SetActive(false);
        statInfoTip.gameObject.SetActive(false);
    }

    public void Start()
    {
        SwitchTo(inGameUI);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);
        
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);
        
        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillsUI);
        
        if (Input.GetKeyDown(KeyCode.B))
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

        if (_menu != null)
        {
            AudioManager.instance.PlaySFx(0, null);
            _menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if(_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
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
    public void LoadData(GameData data)
    {
        foreach (var pair in data.volumeSettings)
        {
            foreach (var item in volumeSettings)
            {
                if(item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();

        foreach (var item in volumeSettings)
        {
            data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
