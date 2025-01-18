using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drownmancer : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int totalEnemies = 10;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float startDelay = 1f;
    private int enemiesSpawned = 0;
    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (enemiesSpawned >= totalEnemies)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        Vector3 spawnPosition = transform.position + randomOffset;

        GameObject newEmemy=Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        GamePlayManager.Instance.SetBrain(newEmemy.GetComponent<EnemyController>(),Vector3Int.FloorToInt(spawnPosition));
        enemiesSpawned++;
    }
}