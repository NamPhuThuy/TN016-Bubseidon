using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenHUD : UIScreenBase
{
    [Header("Buttons")] 
    [SerializeField] private Button _pauseButton;

    private void Start()
    {
        _pauseButton.onClick.AddListener(OnPauseClick);
    }
    
    private void OnDestroy()
    {
        _pauseButton.onClick.RemoveAllListeners();
    }

    private void OnPauseClick()
    {
        UIManager.Instance.UIScreenPauseGame.Show();
    }

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
