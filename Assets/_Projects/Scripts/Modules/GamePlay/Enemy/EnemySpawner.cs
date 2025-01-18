using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform _spawnP1;
    public Transform _spawnP2;
    public Transform _endP1;
    public Transform _enemyParent;
    
    [SerializeField] private int _wave=0;
    [SerializeField] private bool waveCompleted = false;
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private LevelDesignData _levelDesignData;
    private bool _isSpawning = false;
    [SerializeField] private float _spawnInterval = 2f;

    
    private int CountChildren()
    {
        return _enemyParent.childCount;
        
    }
    private void Start()
    {
        _levelDesignData = DataManager.Instance.LevelDesignData;
        StartCoroutine(NextWave());
    }

    private IEnumerator SpawnEnemy()
    {
        _isSpawning = true;
        waveCompleted = false;
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

        spawnPosition =
            GamePlayManager.Instance._map.GetCellCenterWorld(GamePlayManager.Instance._map.WorldToCell(spawnPosition));
        
        while(i < _levelDesignData._waveList[_wave].enemyList.Count)
        {
            Debug.Log($"Wave {_wave+1}");
            int count=_levelDesignData._waveList[_wave].enemyCountList[i];
            _enemyPrefab = _levelDesignData._waveList[_wave].enemyList[i].GetComponent<EnemyController>();
            for (int j=0;j< count;j++)
            {
                EnemyController newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<EnemyController>();
                
                newEnemy._startPos = GamePlayManager.Instance._map.WorldToCell(spawnPosition);
                newEnemy._endPos = GamePlayManager.Instance._map.WorldToCell(_endP1.position);
                newEnemy.transform.SetParent(_enemyParent);
                yield return new WaitForSeconds(_spawnInterval);
            }
            i++;
        }
        Debug.Log("All enemies spawned");
        _isSpawning = false;
    }
    private IEnumerator WaveTimer(float waveDuration)
    {
        float elapsed = 0f;
        while (elapsed < waveDuration)
        {
            if(CountChildren() == 0 && _isSpawning == false)
            {
                Debug.Log("Wave Completed");
                waveCompleted = true;
                break;
            }
            elapsed += Time.deltaTime;
            // Debug.Log($"Wave Timer: {elapsed}/{waveDuration}");
            yield return null;
        }
        waveCompleted = true;
    }

    private IEnumerator NextWave()
    {
        Debug.Log("Called NextWave()");
        while (_wave < _levelDesignData._waveList.Count)
        {
            Debug.Log($"Starting Wave {_wave + 1}");
            
            float waveDuration = _levelDesignData._waveList[_wave].waveDuration;
            _spawnInterval = _levelDesignData._waveList[_wave].spawnInterval;
            StartCoroutine(SpawnEnemy());
            yield return StartCoroutine(WaveTimer(waveDuration));
            while(!waveCompleted)
            {
                yield return null;
            }
            Debug.Log($"Wave {_wave} completed. Preparing for the next wave...");
            
            yield return new WaitForSeconds(_levelDesignData._waveList[_wave].waveDelay);
            _wave++;
            Debug.Log("All Waves Completed!");  
        }
        
        MessageManager.Instance.SendMessage(new Message(NamMessageType.OnGameWin));
    }
}
