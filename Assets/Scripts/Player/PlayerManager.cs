using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour , ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int souls;

    private void Awake()
    {
        // to make sure only one instance
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    // money function
    public bool HaveEnoughMoney(int price)
    {
        if (price > souls)
        {
            Debug.Log("Not enough money");
            return false;
        }
        souls -= price;
        return true;
    }

    public int GetSouls() => souls;
    public void LoadData(GameData data)
    {
        // get data
        this.souls = data.souls;
    }

    public void SaveData(ref GameData data)
    {
        // save data
        data.souls = this.souls;
    }
}
