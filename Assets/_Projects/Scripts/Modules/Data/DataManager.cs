using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public PlayerData PlayerData;
    public LevelDesignData LevelDesignData;
    public TowerData TowerData;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateData()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Wave", 0);
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.SetInt("BaseHP", 0);
        PlayerPrefs.Save();
    }
    
    public void LoadData()
    {
        PlayerData.score = PlayerPrefs.GetInt("Score", 0);
        PlayerData.wave = PlayerPrefs.GetInt("Wave", 0);
        PlayerData.coin = PlayerPrefs.GetInt("Coin", 0);
        PlayerData.baseHP = PlayerPrefs.GetInt("BaseHP", 0);
    }
    
    public void SaveData()
    {
        PlayerPrefs.SetInt("Score", PlayerData.score);
        PlayerPrefs.SetInt("Wave", PlayerData.wave);
        PlayerPrefs.SetInt("Coin", PlayerData.coin);
        PlayerPrefs.SetInt("BaseHP", PlayerData.baseHP);
        PlayerPrefs.Save();
    }
    
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
    
    
}
