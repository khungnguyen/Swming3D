using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCREEN
{
    None,
    MainMenu,
    Main
}
public class ScenesManager
{
    private SCREEN curScene = SCREEN.None;

    private static ScenesManager _instance;
    public static ScenesManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScenesManager();
            }
            return _instance;
        }
    }
    private ScenesManager()
    {

    }
    /*
     * Load Scene 
     * newScene : SCREEN enum
     * useSynchornizeClienst : Will affect to other clients by using photon 
     */
    public void GoTo(SCREEN  newScene,bool useSynchornizeClienst = false)
    {
        Debug.LogError("Go to scene " + newScene.ToString() +"- CurrenScene "+ curScene.ToString());
        if(curScene != newScene)
        {
            string sceneName = newScene.ToString();
            if (useSynchornizeClienst)
            {
                PhotonNetwork.LoadLevel(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
            curScene = newScene;
        }

    }
}
