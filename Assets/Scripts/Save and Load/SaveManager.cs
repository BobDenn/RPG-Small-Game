using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private GameData gameData;
    private List<ISaveManager> _saveManagers;
    private FileDataHandler _fileDataHandler;

    [SerializeField] private string fileName;
    
    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    
    // load game data
    private void Start()
    {
                                                //文 件 路 径
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        _saveManagers = FindAllSaveManagers();
        
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = _fileDataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager manager in _saveManagers)
        {
            manager.LoadData(gameData);
        }

        //Debug.Log("Loaded souls " + gameData.souls);
    }
    
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    
    public void SaveGame()
    {
        // data handler save gameData
        foreach (ISaveManager manager in _saveManagers)
        {
            manager.SaveData(ref gameData);
        }
        _fileDataHandler.Save(gameData);
        //Debug.Log("Saved souls " + gameData.souls);
    }

    // need to learn
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
