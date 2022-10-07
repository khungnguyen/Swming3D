using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class MatchManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
