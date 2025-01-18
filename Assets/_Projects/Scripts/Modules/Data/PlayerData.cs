using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int score = 0;
    public int coin = 0;
    public int currentWave = 0;
    public int wave = 0;
    public float baseHP = 10f;
    public float currentHP = 10f;
}
