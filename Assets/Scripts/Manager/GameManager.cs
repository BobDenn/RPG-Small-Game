using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closestCheckpointId;
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
        
    }

    public void RestartGame()
    {
        SaveManager.instance.SaveGame();// 在重启游戏时也保存一次游戏
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void SaveData(ref GameData data)
    {
        data.closestCheckpointId = FindClosestCheckPoint().id;
        data.checkPoints.Clear();
        
        foreach (CheckPoint checkPoint in checkPoints)
        {
            data.checkPoints.Add(checkPoint.id, checkPoint.status);
        }
    }
    
    public void LoadData(GameData data)
    {
        foreach (var pair in data.checkPoints)
        {
            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key && pair.Value)
                    checkPoint.ActivateCheckPoint();
            }
        }

        closestCheckpointId = data.closestCheckpointId;
        //Debug.Log("获取了最近的复活点");
        Invoke("RespawnOnCheckpoint", .1f);// delay .1f to run
//        Debug.Log("GameManager Loaded");
    }

    public void RespawnOnCheckpoint()
    {
        foreach (var point in checkPoints)
        {
            if(closestCheckpointId == point.id)
                //player respawn near the check point
                PlayerManager.instance.player.transform.position = point.transform.position;
        }
    }

    private CheckPoint FindClosestCheckPoint()
    {
        float shortDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach (var point in checkPoints)
        {
            float distanceToPoint =
                Vector2.Distance(PlayerManager.instance.player.transform.position, point.transform.position);
            if (distanceToPoint < shortDistance && point.status)
            {
                shortDistance = distanceToPoint;
                closestCheckpoint = point;
            }
        }
        return closestCheckpoint;
    }
    
}
