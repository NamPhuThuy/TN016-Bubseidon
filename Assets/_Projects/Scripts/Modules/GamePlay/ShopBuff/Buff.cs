using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Buff : ScriptableObject
{
    [SerializeField]public string title;
    [SerializeField]public string description;
    [SerializeField]public int baseCost;
    [SerializeField]public Image img;
    
    public int boughtNums;//each buff is bought how many time
    public void restart(){
        boughtNums=0;
    }
    public virtual void Active(){}
}
