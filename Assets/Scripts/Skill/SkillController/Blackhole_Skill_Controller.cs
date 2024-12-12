using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    [Header("black hole info")]
    public float maxSize;
    public float groupSpeed;
    public bool canGrow;
    public bool canShrink;
    public float shrinkSpeed;

    [Header("clone attack info")]
    public int amountOfAttack = 4;
    public float cloneAttackCooldown = .3f;
    private float _cloneAttackTimer;
    private bool _cloneCanAttack;
    private bool _canCreateHotKeys = true;
    
    private List<Transform> _targets = new List<Transform>();
    private List<GameObject> _createHotKey = new List<GameObject>();

    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime;
        // press R use ultimate ability -0-
        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyHotKeys();
            _cloneCanAttack = true;
            _canCreateHotKeys = false;
        }
        
        if (_cloneAttackTimer < 0 && _cloneCanAttack)
        {
            _cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, _targets.Count);
            
            // use offset
            float xOffset;
            if (Random.Range(0, 100) > 40)
                xOffset = 2;
            else
                xOffset = -2;
                
            SkillManager.instance.clone.CreateClone(_targets[randomIndex], new Vector3(xOffset, 0));
            
            amountOfAttack--;
            if (amountOfAttack < 0)
            {
                canShrink = true;
                _cloneCanAttack = false;
            }

        }
        
        
        if (canGrow && !canShrink)
        {
            // expand black hole
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                groupSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            // if black hole is too small then destroy
            if(transform.localScale.x < 0)
                Destroy(gameObject);
        }

    }

    // destroy hot keys
    private void DestroyHotKeys()
    {
        if(_createHotKey.Count<0)
            return;
        for (int i = 0; i < _createHotKey.Count; i++)
        {
            Destroy(_createHotKey[i]);
        }
    }

    // detect enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            // freeze enemy and Initialize hotkey
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("no one hotkey is valuable");
            return;
        }
        
        if(!_canCreateHotKeys)
            return;
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2),
            Quaternion.identity);
        
        _createHotKey.Add(newHotKey);
            
        // got random key
        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        Blackhole_Hotkey_Controller newHotkeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();
        // assign    
        newHotkeyScript.SetupHotKey(chooseKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform enemyTransform)
    {
        _targets.Add(enemyTransform);
        //_targets.Add(enemyTransform);
    } 
}