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
    //game pauseScreen, 
    
    //when the GameManager.OnSceneLoad triggerd turn on UIHUD
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        MessageManager.Instance.AddSubcriber(NamMessageType.OnDataChanged, this);
        UIScreenTitle.Show();
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnDataChanged, this);
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case NamMessageType.OnDataChanged:
                UIScreenHUD.UpdateUI();
                break;
        }
    }
}
