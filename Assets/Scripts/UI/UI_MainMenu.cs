using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "Game";
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if(SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavaData();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Gaming");
        //Application.Quit();
    }
}
