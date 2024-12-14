using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer _sr;
    private KeyCode _myHotKey;
    private TextMeshProUGUI _myText;

    private Transform _myEnemies;
    private Blackhole_Skill_Controller _blackHole;

    public void SetupHotKey(KeyCode myNewHotKey, Transform myEnemy, Blackhole_Skill_Controller blackHole)
    {
        _sr = GetComponent<SpriteRenderer>();
        
        // assign hotkey that we've got
        _myText = GetComponentInChildren<TextMeshProUGUI>();

        _myEnemies = myEnemy;
        _blackHole = blackHole;
        
        _myHotKey = myNewHotKey;
        _myText.text = myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_myHotKey))
        {
            // attack enemy when we press right key
            _blackHole.AddEnemyToList(_myEnemies);

            _myText.color = Color.clear;
            _sr.color = Color.clear;
        }
    }
}
