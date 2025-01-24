using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerData
{
    public int score = 0;
    public int coin = 0;
    public int currentWave = 0;
    public int wave = 0;
    
    //HP
    public float maxHP = 50f;
    public float currentHP = 50f;
    
    //MP
    public float maxMP = 100f;
    public float currentMP = 100f;
    public float mpDecreaseSpeed = 45f;
    public float mpIncreaseSpeed = 25f;
}
