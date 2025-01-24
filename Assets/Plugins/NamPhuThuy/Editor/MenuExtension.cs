using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
#endif

#if UNITY_EDITOR
public class MenuExtension : Editor
{
    const string SWITCH_SCENCE_MENU_NAME = "Tools/Switch Scene";
    private const string PATH_TO_SCENES_FOLDER = "Assets/_GamePrototype2D/Scenes/";
    
    private const string ALT = "&";



    [MenuItem("GameObject/Remove Missing Scripts")]
    public static void Remove()
    {
        var objs = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = objs.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        Debug.Log($"Removed {count} missing scripts");
    }

    #region LoadSceneShortCut
    [MenuItem(SWITCH_SCENCE_MENU_NAME + "/Bootstrap " + ALT + "1")]
    static void Boots() { LoadSceneByName("Bootstrap"); }


    [MenuItem(SWITCH_SCENCE_MENU_NAME + "/MainMenu " + ALT + "2")]
    static void MainMenu() { LoadSceneByName("MainMenu"); }
    
    [MenuItem(SWITCH_SCENCE_MENU_NAME + "/GamePlay " + ALT + "3")]
    static void Gameplay() { LoadSceneByName("GamePlay"); }
    
    static void LoadSceneByName(string _nameScene)
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene($"{PATH_TO_SCENES_FOLDER}{_nameScene}.unity");
    }

    #endregion

}
#endif
