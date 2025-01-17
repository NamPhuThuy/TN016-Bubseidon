using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenTitle : UIScreenBase
{
    [Header("Buttons")]
    [SerializeField] private Button _clickToStartButton;
    
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void Start()
    {
        _clickToStartButton.onClick.AddListener(OnClickToStart);
    }

    private void OnDestroy()
    {
        _clickToStartButton.onClick.RemoveListener(OnClickToStart);
    }

    #region Button events

    private void OnClickToStart()
    {
        MessageManager.Instance.SendMessage(new Message(NamMessageType.OnGameStart));
        UIManager.Instance.UIScreenHUD.Show();
        Hide();
    }

    #endregion
}
