using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public GameData gameData;
    private List<ISaveManager> _saveManagers;
    private FileDataHandler _fileDataHandler;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    
    [ContextMenu("Delete save file")]
    public void DeleteSavaData()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        _fileDataHandler.Delete();
    }
    
    private void Awake()
    {
        
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        
        //文 件 路 径
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        _saveManagers = FindAllSaveManagers();
        
    }
    
    private void Start()
    {
        LoadGame();
        //Debug.Log("加载完毕！");
    }
    public void LoadGame()
    {
        gameData = _fileDataHandler.Load();

        if (this.gameData == null)
        {
            //Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager manager in _saveManagers)
        {
            manager.LoadData(gameData);
        }

        //Debug.Log("Loaded souls " + gameData.souls);
    }
    

    public void NewGame()
    {
        gameData = new GameData();
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
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (_fileDataHandler.Load() != null)
            return true;

        return false;
    }
}
