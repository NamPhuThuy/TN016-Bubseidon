using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Screen")]
    public UIScreenHUD UIScreenHUD;
    public UIScreenPauseGame UIScreenPauseGame;
    public UIScreenSettings UIScreenSettings;
    //game pauseScreen, 
    
    //when the GameManager.OnSceneLoad triggerd turn on UIHUD
    private void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
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
                UIScreenHUD.Show();
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
}
