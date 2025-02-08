using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform _player;
    
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closestCheckpointId;
    
    [Header("Lost souls")]
    [SerializeField] private GameObject lostSoulsPrefab;
    public int lostSoulsAmount;
    [SerializeField] private float lostSoulsX;
    [SerializeField] private float lostSoulsY;
    
    private void Awake()
    {
        // to make sure only one instance
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        
        checkPoints = FindObjectsOfType<CheckPoint>();
        //Debug.Log("找到了检查点");
    }

    private void Start()
    {
        _player = PlayerManager.instance.player.transform;
    }

    public void RestartGame()
    {
        SaveManager.instance.SaveGame();// 在重启游戏时也保存一次游戏
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void SaveData(ref GameData data)
    {
        data.lostSoulsAmount = lostSoulsAmount;
        data.lostSoulsX = _player.position.x;
        data.lostSoulsY = _player.position.y;
        
        if(FindClosestCheckPoint() != null)
            data.closestCheckpointId = FindClosestCheckPoint().id;
        data.checkPoints.Clear();
        
        foreach (CheckPoint checkPoint in checkPoints)
        {
            data.checkPoints.Add(checkPoint.id, checkPoint.status);
        }
    }
    
    public void LoadData(GameData data) => StartCoroutine(LoadWithDelay(data));

    private void LoadCheckPoints(GameData data)
    {
        foreach (var pair in data.checkPoints)
        {
            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key && pair.Value)
                    checkPoint.ActivateCheckPoint();
            }
        }
    }

    private void LoadLostSouls(GameData data)
    {
        lostSoulsAmount = data.lostSoulsAmount;
        lostSoulsX = data.lostSoulsX;
        lostSoulsY = data.lostSoulsY;

        if (lostSoulsAmount > 0)
        {
            GameObject newLostSouls =
                Instantiate(lostSoulsPrefab, new Vector3(lostSoulsX, lostSoulsY), Quaternion.identity);
            newLostSouls.GetComponent<LostItems>().remainingSouls = lostSoulsAmount;
        }
        
        lostSoulsAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData data)
    {
        yield return new WaitForSeconds(.1f);
        LoadCheckPoints(data);
        
        RespawnOnCheckpoint(data);
        
        LoadLostSouls(data); 
    }
    
    public void RespawnOnCheckpoint(GameData data)
    {
        if (data.closestCheckpointId == null)
            return;
        
        closestCheckpointId = data.closestCheckpointId;
        
        foreach (var point in checkPoints)
        {
            if(closestCheckpointId == point.id)
                //player respawn near the check point
                _player.position = point.transform.position;
        }
    }

    private CheckPoint FindClosestCheckPoint()
    {
        float shortDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach (var point in checkPoints)
        {
            float distanceToPoint =
                Vector2.Distance(_player.position, point.transform.position);
            if (distanceToPoint < shortDistance && point.status)
            {
                shortDistance = distanceToPoint;
                closestCheckpoint = point;
            }
        }
        return closestCheckpoint;
    }
    
}
