using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : MonoBehaviour, IMessageHandle
{
    public PlayerData PlayerData;
    public LevelDesignData LevelDesignData;
    public TowerData TowerData;
    public static DataManager Instance;
    
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
        get => PlayerData.baseHP;
        set
        {
            PlayerData.baseHP = value;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            SaveData();
        }
    }
    
    public float CurrentHP
    {
        get => PlayerData.baseHP;
        set
        {
            PlayerData.currentHP = value;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            SaveData();
        }
    }
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerData = new PlayerData();
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(NamMessageType.OnEnemyDie, this);
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnEnemyDie, this);
    }

    public void CreateData()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Wave", 0);
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.SetInt("BaseHP", 10);
        PlayerPrefs.SetInt("CurrentHP", 10);
        PlayerPrefs.Save();
    }
    
    public void LoadData()
    {
        PlayerData.score = PlayerPrefs.GetInt("Score", 0);
        PlayerData.currentWave = PlayerPrefs.GetInt("Wave", 0);
        PlayerData.coin = PlayerPrefs.GetInt("Coin", 0);
        PlayerData.baseHP = PlayerPrefs.GetInt("BaseHP", 10);
        PlayerData.baseHP = PlayerPrefs.GetInt("CurrentHP", 10);
    }
    
    public void SaveData()
    {
        PlayerPrefs.SetInt("Score", PlayerData.score);
        PlayerPrefs.SetInt("Wave", PlayerData.currentWave);
        PlayerPrefs.SetInt("Coin", PlayerData.coin);
        PlayerPrefs.SetFloat("BaseHP", PlayerData.baseHP);
        PlayerPrefs.SetFloat("CurrentHP", PlayerData.currentHP);
        PlayerPrefs.Save();
    }
    
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }


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