using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : Singleton<DataManager>, IMessageHandle
{
    public PlayerData PlayerData;
    public LevelDesignData LevelDesignData;
    public TowerDatas TowerDatas;
    public EnemyDatas EnemyDatas;
    


    //Automatically increase coin through time
    private float _timer = 0f;
    private float _timeToCoin = 5f;

    #region PlayerData Properties

    public int Coin
    {
        get => PlayerData.coin;
        set
        {
            PlayerData.coin = value;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            SaveData();
        }
    }

    public int Score
    {
        get => PlayerData.score;
        set
        {
            PlayerData.score = value;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            SaveData();
        }
    }

    public float BaseHP
    {
        get => PlayerData.maxHP;
        set
        {
            PlayerData.maxHP = value;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            SaveData();
        }
    }
    
    public float CurrentHP
    {
        get => PlayerData.currentHP;
        set
        {
            PlayerData.currentHP = value;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            if (CurrentHP <= 0)
            {
                MessageManager.Instance.SendMessage(new Message(NamMessageType.OnGameLose));
            }
            SaveData();
        }
    }

    #endregion

    #region EnemyData Methods

    public EnemyData GetEnemyDataById(string id)
    {
        foreach (var enemyData in EnemyDatas._enemyDatas)
        {
            if (enemyData.id == id)
            {
                return enemyData;
            }
        }
        return new EnemyData();
    }

    #endregion
    
    #region MonoBehaviour Methods
    void Start()
    {
        PlayerData = new PlayerData();
        LoadData();
    }

    private void Update()
    {
        if (_timer >= _timeToCoin)
        {
            Coin += 1;
            _timer = 0f;
        }
        _timer += Time.deltaTime;
    }

    private void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(NamMessageType.OnEnemyDie, this);
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnEnemyDie, this);
    }
    #endregion

    #region DataManager Methods

    public void CreateData()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.Save();
    }
    
    public void LoadData()
    {
        // PlayerData.score = PlayerPrefs.GetInt("Score", 0);
        // PlayerData.coin = PlayerPrefs.GetInt("Coin", 0);
    }
    
    public void SaveData()
    {
        // PlayerPrefs.SetInt("Score", PlayerData.score);
        // PlayerPrefs.SetInt("Coin", PlayerData.coin);
        PlayerPrefs.Save();
    }
    
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion


    public void Handle(Message message)
    {
        switch (message.type)
        {
            case NamMessageType.OnEnemyDie:
                Score += 10;
                break;
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(DataManager)), CanEditMultipleObjects]
public class DataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataManager dataManager = (DataManager)target;

        if (GUILayout.Button("Save Data"))
        {
            dataManager.SaveData();
        }

        if (GUILayout.Button("Reset Data"))
        {
            dataManager.ResetData();
        }

        if (GUILayout.Button("Load Data"))
        {
            dataManager.LoadData();
        }
    }
}
#endif