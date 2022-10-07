using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerSpawner instance;
    private GameObject player;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            {
                Transform point = SpawnPointManager.instance.GetStudentSpawnPoint();
              var gameObject =  PhotonNetwork.Instantiate("Student", point.transform.position, point.transform.rotation);
                gameObject.transform.SetParent(point);
            }
        }
    }

    public void SpawnPlayer()
    {
        if(DectectVR.instancne.isVR)
        {
            Transform transfrom = SpawnPointManager.instance.GetPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate("Coach", transfrom.position, transform.rotation);
        }
        else
        {
            Transform transfrom = SpawnPointManager.instance.GetPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate("Player", transfrom.position, transform.rotation);
        }
        //Transform transfrom = SpawnPointManager.instance.GetPlayerSpawnPoint();
        //player = PhotonNetwork.Instantiate("Coach", transfrom.position, transform.rotation);
        //Transform transfrom1 = SpawnPointManager.instance.GetPlayerSpawnPoint();
        //player = PhotonNetwork.Instantiate("Player", transfrom1.position, transfrom1.rotation);
    }
}
