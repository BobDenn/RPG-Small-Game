using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "Game";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen _fadeScreen;
    private void Start()
    {
        if(SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeScreen(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavaData();
        StartCoroutine(LoadSceneWithFadeScreen(1.5f));
    }

    public void ExitGame()
    {
        Debug.Log("Exit Gaming");
        //Application.Quit();
    }

    IEnumerator LoadSceneWithFadeScreen(float delay)
    {
        _fadeScreen.FadeIn();
        
        yield return new WaitForSeconds(delay);
        
        SceneManager.LoadScene(sceneName);
    }
    
}
