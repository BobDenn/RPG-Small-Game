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
    
    private float _maxSize;
    private float _growSpeed;
    private float _shrinkSpeed;
    private float _blackHoleTimer;
    private bool _canGrow = true;
    private bool _canShrink;
    
    private int _amountOfAttacks;
    private float _cloneAttackCooldown = .3f;
    private float _cloneAttackTimer;
    private bool _cloneAttackReleased;
    private bool _canCreateHotKeys = true;
    private bool _playerCanDisappear = true;
    
    private List<Transform> _targets = new List<Transform>();
    private List<GameObject> _createHotKey = new List<GameObject>();
    
    public bool playerCanExitState { get; private set; }
    
    // pass black hole parameters
    public void SetupBlackHole(float maxSize, float growSpeed,float shrinkSpeed, int amountOfAttacks, float cloneAttackCooldown, float blackHoleDuration)
    {
        _maxSize = maxSize;
        _growSpeed = growSpeed;
        _shrinkSpeed = shrinkSpeed;
        _amountOfAttacks = amountOfAttacks;
        _blackHoleTimer = blackHoleDuration;
        _cloneAttackCooldown = cloneAttackCooldown;

        if(SkillManager.instance.clone.crystalInsteadOfClone)
            _playerCanDisappear = false;

    }
    
    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime; // always negative
        _blackHoleTimer -= Time.deltaTime;

        if (_blackHoleTimer < 0)
        {
            _blackHoleTimer = Mathf.Infinity;
            
            if(_targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHole();
        }
        
        // press R use clone attack -0-
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttack();
        
        
        if (_canGrow && !_canShrink)
        {
            // expand black hole slowly
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize),
                _growSpeed * Time.deltaTime);
        }

        if (_canShrink)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);
            // if black hole is too small then destroy
            if(transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if(_targets.Count <= 0)
            return;
        
        DestroyHotKeys();
        _cloneAttackReleased = true;
        _canCreateHotKeys = false;
        if (_playerCanDisappear)
        {
            _playerCanDisappear = false;
            
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }    
    }

    private void CloneAttack()
    {
        if (_cloneAttackTimer < 0 && _cloneAttackReleased && _amountOfAttacks > 0)
        {
            _cloneAttackTimer = _cloneAttackCooldown;
            
            int randomIndex = Random.Range(0, _targets.Count);
            
            // clone's offset position
            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 1;
            else
                xOffset = -1;
            
            if(SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(_targets[randomIndex], new Vector3(xOffset, 0));
            }

            
            _amountOfAttacks--;
            
            // black hole shrinks when attack finished 
            if (_amountOfAttacks <= 0)
            {
                // try out what does invoke mean 
                Invoke("FinishBlackHole", 1f);
                
            }
        }
    }

    private void FinishBlackHole()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        _canShrink = true;
        _cloneAttackReleased = false;
    }

    // destroy hot keys
    private void DestroyHotKeys()
    {
        if(_createHotKey.Count <= 0)
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
    // exit detecting
    /*private void OnTriggerExit(Collider collision)
    {
        if(collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }*/
    // another way to write this function 
    // ? means if it isn't null then do behind function, otherwise do nothing. super easy super cool
    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

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