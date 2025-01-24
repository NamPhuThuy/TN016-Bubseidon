using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public struct EnemyDatas
{
    public static List<EnemyData> _enemyDatas = new List<EnemyData>()
    {
        new EnemyData("Normal", "En001", 2f, 1f, 35f, 1.2f),
        new EnemyData("Fast", "En002", 1f, 0.8f, 24f, 2.4f),
        new EnemyData("Tank", "En003", 5f, 2f, 65f, 0.8f),
        new EnemyData("Boss", "En004", 8f, 3f, 100f, 0.5f)
    };
}

[Serializable]
public struct EnemyData
{
    public string name;
    public string id;
    public float attackDamage;
    public float attackCoolDown;
    public float hp;
    public float runSpeed;
    
    /*public List<string> rewards;
    public List<int> rewardsAmount;*/
    
    public EnemyData(string name, string id, float attackDamage, float attackCoolDown, float hp, float runSpeed)
    {
        this.name = name;
        this.id = id;
        this.hp = hp;
        this.attackDamage = attackDamage;
        this.attackCoolDown = attackCoolDown;
        this.runSpeed = runSpeed;
    }
}
