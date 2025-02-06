using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int souls;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public GameData()
    {
        this.souls = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
    }
}
