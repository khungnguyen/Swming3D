using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public enum SCREEN
{
    MainMenu,
    Main
}
public class MatchManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private SCREEN StartScene = SCREEN.MainMenu;
    void Start()
    {
        if(!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene(StartScene.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
