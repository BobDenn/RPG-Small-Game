using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

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
        if (price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }
        currency -= price;
        return true;
    }
}
