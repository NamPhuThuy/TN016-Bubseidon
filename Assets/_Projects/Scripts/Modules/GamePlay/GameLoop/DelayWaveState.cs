using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class DelayWaveState : ComponentState
{
    private float DelayTime => DataManager.Instance.LevelDesignData
        ._waveList[DataManager.Instance.PlayerData.currentWave].waveDelay;

    private void OnEnable()
    {
        StartCoroutine(DelaySpawn());
    }

    IEnumerator DelaySpawn()
    {
        yield return Yielders.Get(DelayTime);
        ChangeState(Next());
    }
}
