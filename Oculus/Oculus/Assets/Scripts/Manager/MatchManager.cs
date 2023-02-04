using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private SCREEN StartScene = SCREEN.MainMenu;
    void Start()
    {
        if (!VRAppDebug.USE_DEBUG_VR_SINGLE_PREVIEW)
        {
            if (!PhotonNetwork.IsConnected)
            {
                ScenesManager.instance.GoTo(StartScene);
                Debug.Log("GO TO MAIN MENU");
            }


        }
    }
}
