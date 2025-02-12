using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int souls;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkPoints;
    public string closestCheckpointId;

    public float lostSoulsX;
    public float lostSoulsY;
    public int lostSoulsAmount;

    public SerializableDictionary<string, float> volumeSettings;
    
    public GameData()
    {
        this.lostSoulsX = 0;
        this.lostSoulsY = 0;
        this.lostSoulsAmount = 0;
        
        this.souls = 0;
        
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        
        
        closestCheckpointId = string.Empty;
        checkPoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
