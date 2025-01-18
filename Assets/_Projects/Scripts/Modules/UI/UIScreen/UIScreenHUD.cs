using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIScreenHUD : UIScreenBase
{
    [Header("Buttons")] 
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _towerShopButton;
    [SerializeField] private Button _instantBuyTowerButton;

    [SerializeField] private List<GameObject> _buildingItems;
    
    [Header("Information")]
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _hpSlider;
    
    [Header("TowerShop")]
    [SerializeField] private TowerDatas _towerDatas;
    [SerializeField] private TowerShopScrollView _towerShopScrollView;
    private int _coinToBuy = 10;
    private bool _isFirstBuy = true;
    [SerializeField] private TextMeshProUGUI _towerPriceText;
    
    

    private void Start()
    {
        _pauseButton.onClick.AddListener(OnPauseClick);
        _towerShopButton.onClick.AddListener(OnTowerShopClick);
        _instantBuyTowerButton.onClick.AddListener(OnInstantBuyTowerClick);
    }

    private void OnInstantBuyTowerClick()
    {
        if (DataManager.Instance.Coin >= _coinToBuy)
        {
            //mua
            int rand = Random.Range(0, _buildingItems.Count);

            if (_isFirstBuy)
            {
                rand = 3;
                _isFirstBuy = false;
            }
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            Instantiate(_buildingItems[rand], player.transform.position, Quaternion.identity);
            DataManager.Instance.Coin -= _coinToBuy;
            _coinToBuy++;
            _towerPriceText.text = "" + _coinToBuy;
        }
    }

    private void OnEnable()
    {
        UpdateUI();
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


    public void UpdateUI()
    {
        Debug.LogError($"Update UI HUD");
        _coinText.text = $"{DataManager.Instance.PlayerData.coin}";
        _scoreText.text = $"{DataManager.Instance.PlayerData.score}";
        
        Debug.LogError($"current hp: {DataManager.Instance.CurrentHP}, base hp: {DataManager.Instance.BaseHP}");
        _hpSlider.value = DataManager.Instance.CurrentHP / DataManager.Instance.BaseHP;
    }
}
