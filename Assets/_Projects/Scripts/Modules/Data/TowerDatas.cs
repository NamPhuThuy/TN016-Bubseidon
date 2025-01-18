using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData", order = 1)]
public class TowerDatas : ScriptableObject
{
    public List<TowerData> towerDatas;
}

[Serializable]
public class TowerData
{
    public string name;
    public int HP;
    public Sprite image;
    public int cost;
    public int damage;
}