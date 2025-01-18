using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuff : MonoBehaviour
{
    public List<ShopBuffItemView> listItemView;
    public ShopBuffRerollView rerollView;
    public List<Buff> listBuff;
    private Buff []currentBuff = new Buff[3];
    private float costIncsRate = 1.5f;
    private int baseRerollCost = 10;
    private int rerollNumber=0;//how many time reroll
    // public int gold=999;
    private void Start() {
        listItemView[0].btn.onClick.AddListener(()=>chooseABuff(0));
        listItemView[1].btn.onClick.AddListener(()=>chooseABuff(1));
        listItemView[2].btn.onClick.AddListener(()=>chooseABuff(2));
        rerollView.btn.onClick.AddListener(()=>reroll());
        restart();
    }
    public void restart(){
        foreach(Buff b in listBuff){
            b.boughtNums=0;
        }
        randomAllBuff();
        reloadShow();
    }
    public int getCostReroll(){
        return (int)(baseRerollCost*MathF.Pow(costIncsRate,rerollNumber));
    }
    public int getCostABuff(int index){
        return (int)(currentBuff[index].baseCost*MathF.Pow(costIncsRate,currentBuff[index].boughtNums));
    }
    public void reroll(){
        int cost = getCostReroll();
        if(DataManager.Instance.PlayerData.coin >cost){
            rerollNumber++;
            DataManager.Instance.PlayerData.coin-=cost;
            randomAllBuff();
        }
        reloadShow();
    }
    public void chooseABuff(int index){
        int cost = getCostABuff(index);
        if(DataManager.Instance.PlayerData.coin>cost){
            currentBuff[index].Active();
            currentBuff[index].boughtNums++;
            DataManager.Instance.PlayerData.coin-=cost;
            randomABuff(index);
        }
        reloadShow();
    }
    private void randomAllBuff(){
        for(int i=0;i<3;i++){
            randomABuff(i);
        }
    }
    private void randomABuff(int index){
        currentBuff[index]=listBuff[UnityEngine.Random.Range(0,listBuff.Count)];
    }
    public void reloadShow(){
        for(int i=0;i<3;i++){
            listItemView[i].Title.text=currentBuff[i].title;
            listItemView[i].Info.text=currentBuff[i].description;
            listItemView[i].Cost.text=getCostABuff(i)+"G";
            listItemView[i].Level.text="Lv. "+(currentBuff[i].boughtNums+1);
        }
        rerollView.Cost.text=getCostReroll()+"G";
    }
}
[Serializable]
public class ShopBuffItemView{
    public Button btn;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Info;
    public Image Img;
    public TextMeshProUGUI Cost;
    public TextMeshProUGUI Level;
}
[Serializable]
public class ShopBuffRerollView{
    public Button btn;
    public TextMeshProUGUI Cost;
}