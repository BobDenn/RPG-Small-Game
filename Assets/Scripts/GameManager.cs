using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private void Awake()
    {
        // to make sure only one instance
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    
    
    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
