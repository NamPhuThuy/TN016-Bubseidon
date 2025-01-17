using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScreenPauseGame : UIScreenBase
{
    [Header("Buttons")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _settingsButton;

    private void Start()
    {
        _resumeButton.onClick.AddListener(OnClickResume);
        _restartButton.onClick.AddListener(OnClickRestart);
        _settingsButton.onClick.AddListener(OnClickSettings);
    }

    #region Button events

    private void OnClickSettings()
    {
        UIManager.Instance.UIScreenSettings.Show();
    }

    private void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnClickResume()
    {
        Hide();
    }

    #endregion
    
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
