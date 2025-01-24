using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    public Transform _spawnP1;
    public Transform _spawnP2;
    public Transform _endP1;
    public Transform _enemyParent;
    
    //Some Values
    private List<LevelDesign> WaveList => DataManager.Instance.LevelDesignData._waveList;
    private int CurrentWaveId => DataManager.Instance.PlayerData.currentWave;
    private float SpawnIntervalTime => WaveList[CurrentWaveId].spawnInterval;
    private int MaxWave => DataManager.Instance.LevelDesignData.maxWave;
    
    [SerializeField] private bool waveCompleted = false;
    [SerializeField] private LevelDesignData _levelDesignData;
    private bool _isSpawning = false;
    
    private int CountChildren()
    {
        return _enemyParent.childCount;
    }
    private void Start()
    {
        _levelDesignData = DataManager.Instance.LevelDesignData;
        // StartCoroutine(SpawnCurrentWave());
        
        //Spawn the current Wave 
        //If the current wave-enemy is cleared -> Start the next Wave
        // If the length of current wave is over -> Start the next Wave
        
    }
    
    public IEnumerator SpawnCurrentWave()
    {
        LevelDesign waveData = DataManager.Instance.LevelDesignData._waveList[CurrentWaveId];

        int totalEnemies = 0;
        foreach (int i in waveData.enemyCountList)
        {
            totalEnemies += i;
        }

        while (totalEnemies > 0)
        {
            
            Vector3 spawnPosi = GetRandomSpawnPosi();
            for (int i = 0; i < WaveList[CurrentWaveId].enemyCountList.Count; i++)
            {
                for (int j = 0; j < WaveList[CurrentWaveId].enemyCountList[i]; j++)
                {
                    yield return Yielders.Get(SpawnIntervalTime);
                    GameObject newEnemy = Instantiate(WaveList[CurrentWaveId].enemyList[i], spawnPosi, Quaternion.identity);
                    newEnemy.transform.parent = _enemyParent.transform;
                    newEnemy.GetComponent<EnemyController>().Setup(GamePlayManager.Instance._map.WorldToCell(spawnPosi), GamePlayManager.Instance._map.WorldToCell(_endP1.position));
                    totalEnemies--;
                }
            }
        }
        yield return null;
    }

    private Vector3 GetRandomSpawnPosi()
    {
        Vector3 spawnPosition = Vector3.zero;
        int i=0;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            spawnPosition = _spawnP1.position;
        }
        else
        {
            spawnPosition = _spawnP2.position;
        }
        
        spawnPosition = GamePlayManager.Instance._map.GetCellCenterWorld(GamePlayManager.Instance._map.WorldToCell(spawnPosition));
        return spawnPosition;
    }
}
