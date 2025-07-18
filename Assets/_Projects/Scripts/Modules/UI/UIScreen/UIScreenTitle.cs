using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenTitle : UIScreenBase
{
    [Header("Buttons")]
    [SerializeField] private Button _clickToStartButton;
    
    [SerializeField] private TextMeshProUGUI _titleText;
    public float flickerInterval = 0.3f; // Time between flickers

    private bool isVisible = true;
    
    public override void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(Flicker());
    }
    
    private IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(flickerInterval);

            // Toggle visibility
            isVisible = !isVisible;
            _titleText.enabled = isVisible;
        }
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
