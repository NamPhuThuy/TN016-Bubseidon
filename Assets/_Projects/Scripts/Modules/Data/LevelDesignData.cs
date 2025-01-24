using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "ScriptableObjects/LevelDesignData", order = 1)]
public class LevelDesignData : ScriptableObject
{
    public List<LevelDesign> _waveList = new List<LevelDesign>();
    public int maxWave => _waveList.Count;
}


[Serializable]
public class LevelDesign
{
    public float waveDelay;
    /// <summary>
    /// The total time the wave last 
    /// </summary>
    public float waveDuration;
    public float spawnInterval;
    // public List<KeyValuePair<GameObject, int>> enemyList = new List<KeyValuePair<GameObject, int>>();
    /// <summary>
    /// List of enemy-prefabs
    /// </summary>
    public List<GameObject> enemyList = new List<GameObject>();
    
    /// <summary>
    /// Number of each enemy-type will be spawned
    /// </summary>
    public List<int> enemyCountList = new List<int>();
    
}
