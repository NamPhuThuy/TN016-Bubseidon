using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class SpawnEnemyState : ComponentState
{
    [SerializeField] private DelayWaveState _delayWaveState;
    [SerializeField] private float _timer = 0f;

    private void OnEnable()
    {
        StartCoroutine(SpawnNextWave());
        _timer = 0f;
    }   

    IEnumerator SpawnNextWave()
    {
        
        StartCoroutine(GamePlayManager.Instance._enemySpawner.SpawnCurrentWave());
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= DataManager.Instance.LevelDesignData._waveList[DataManager.Instance.PlayerData.currentWave].waveDuration)
        {
            if (DataManager.Instance.PlayerData.currentWave < DataManager.Instance.LevelDesignData.maxWave)
            {
                DataManager.Instance.PlayerData.currentWave++;
                ChangeState(_delayWaveState.gameObject);
            }
            else
            {
                ChangeState(Next());
            }
        };
    }
}
