using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIScreenHUD : UIScreenBase
{
    [Header("Buttons")] 
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _towerShopButton;
    
    
    [Header("TowerShop")]
    [SerializeField] private TowerDatas _towerDatas;
    [SerializeField] private TowerShopScrollView _towerShopScrollView;

    private void Start()
    {
        _pauseButton.onClick.AddListener(OnPauseClick);
        _towerShopButton.onClick.AddListener(OnTowerShopClick);
    }

    

    private void OnDestroy()
    {
        _pauseButton.onClick.RemoveAllListeners();
        _towerShopButton.onClick.RemoveAllListeners();
    }

    #region Button events

    private void OnPauseClick()
    {
        UIManager.Instance.UIScreenPauseGame.Show();
    }
    
    private void OnTowerShopClick()
    {
        if (_towerShopScrollView.gameObject.activeSelf)
            _towerShopScrollView.Hide();
        else
            _towerShopScrollView.Show();
    }

    #endregion

    public override void Show()
    {
        gameObject.SetActive(true);
        //cap nhat data 
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        //
    }

    
}
