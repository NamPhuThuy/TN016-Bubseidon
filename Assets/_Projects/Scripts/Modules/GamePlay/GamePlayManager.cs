using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamePlayManager : Singleton<GamePlayManager>, IMessageHandle
{
    [SerializeField] private PlayerController _player;

    public PlayerController Player
    {
        get => _player;
        set => _player = value;
    }

    [SerializeField] private List<TowerController> _towerList;
    [SerializeField] private List<EnemyController> _enemyList;
    [SerializeField] private EnemySpawner _enemySpawner;
    
    [Header("Resources")]
    public CoinController _coinController;


    [Header("Level Design")] 
    public Tilemap _map;
    public List<EnemyController> EnemyList
    {
        get => _enemyList;
    }


    private void OnEnable()
    {
        // _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _player.gameObject.SetActive(false);
        _enemySpawner.gameObject.SetActive(false);
        
        MessageManager.Instance.AddSubcriber(NamMessageType.OnGameStart, this);
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnGameStart, this);
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

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case NamMessageType.OnGameStart:
                StartTheGame();
                break;
        }
    }

    private void StartTheGame()
    {
        _player.gameObject.SetActive(true);
        _enemySpawner.gameObject.SetActive(true);
    }
    public void SetBrain(EnemyController enemy, Vector3Int spawnPos)
    {
        
        enemy._endPos = _map.WorldToCell(_enemySpawner._endP1.position);
        enemy.transform.SetParent(_enemySpawner._enemyParent);
        enemy.BackToPath(spawnPos);    
    }
}
