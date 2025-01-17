using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamePlayManager : Singleton<GamePlayManager>
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private List<TowerController> _towerList;
    [SerializeField] private List<EnemyController> _enemyList;
    [SerializeField] private EnemySpawner _enemySpawner;


    [Header("Level Design")] 
    public Tilemap _map;
    public List<EnemyController> EnemyList
    {
        get => _enemyList;
    }

    public PlayerController Player
    {
        get => _player;
    }
    
    public event Action OnEnemyDied;
    public event Action OnEnemySpawned;


    private void OnEnable()
    {
        // _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void AddTower(TowerController towerController)
    {
        Debug.Log($"Add tower {towerController.name} to list");
        _towerList.Add(towerController);
    }

    public void RemoveTower(TowerController towerController)
    {
        if (_towerList.Contains(towerController))
            _towerList.Remove(towerController);
    }

    public void AddEnemy(EnemyController enemyController)
    {
        Debug.Log($"Add enemy {enemyController.name} to list");
        _enemyList.Add(enemyController);
    }

    public void RemoveEnemy(EnemyController enemyController)
    {
        if (_enemyList.Contains(enemyController))
            _enemyList.Remove(enemyController);
    }
}
