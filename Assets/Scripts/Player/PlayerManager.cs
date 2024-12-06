using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        // to make sure only one instance
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
