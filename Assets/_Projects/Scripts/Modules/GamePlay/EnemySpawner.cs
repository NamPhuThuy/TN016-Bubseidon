using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnP1;
    [SerializeField] private Transform _spawnP2;
    [SerializeField] private Transform _endP1;
    
    
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private float _spawnInterval = 2f;

    
    
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        Vector3 spawnPosition = Vector3.zero;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            spawnPosition = _spawnP1.position;
        }
        else
        {
            spawnPosition = _spawnP2.position;
        }
        
        EnemyController newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<EnemyController>();
        
        Debug.LogError($"Whut");
        
        newEnemy._startPos = GamePlayManager.Instance._map.WorldToCell(spawnPosition);
        newEnemy._endPos = GamePlayManager.Instance._map.WorldToCell(_endP1.position);
        yield return new WaitForSeconds(_spawnInterval);
    }
}
