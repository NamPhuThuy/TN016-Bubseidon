using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>, IMessageHandle
{
    [Header("UI Screen")]
    public UIScreenHUD UIScreenHUD;
    public UIScreenPauseGame UIScreenPauseGame;
    public UIScreenSettings UIScreenSettings;
    public UIScreenTitle UIScreenTitle;
    public UIScreenGameOver UIScreenGameOver;
    //game pauseScreen, 
    
    //when the GameManager.OnSceneLoad triggerd turn on UIHUD
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        MessageManager.Instance.AddSubcriber(NamMessageType.OnDataChanged, this);
        MessageManager.Instance.AddSubcriber(NamMessageType.OnGameLose, this);
        MessageManager.Instance.AddSubcriber(NamMessageType.OnGameWin, this);
        UIScreenTitle.Show();
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnDataChanged, this);
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnGameLose, this);
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnGameWin, this);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;  // Get the name of the loaded scene

        // Choose and play the appropriate music based on sceneName
        switch (sceneName)
        {
            case "MainMenu":
               
                break;
            case "GamePlay":
                break;
        }
    }

    public void Handle(Message message)
    {
        Debug.Log($"UIManager handle message {message.type}");
        switch (message.type)
        {
            case NamMessageType.OnDataChanged:
                UIScreenHUD.UpdateUI();
                break;
            case NamMessageType.OnGameLose:
                UIScreenGameOver.Show(false);
                break;
            case NamMessageType.OnGameWin:
                UIScreenGameOver.Show(true);
                break;
            
        }
    }
}
