using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerSpawner instance;
    public GameObject PlayerPrefab;
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
                Transform point = SpawnManager.instance.GetSpawnTranform();
                PhotonNetwork.Instantiate("Student", Vector3.zero, point.transform.rotation);
                Debug.Log("Creating Aniamtion");
            }
        }
    }

    public void SpawnPlayer()
    {
        Transform transfrom = SpawnManager.instance.GetSpawnTranform();
        player = PhotonNetwork.Instantiate("Player", transfrom.position, transform.rotation);
    }
}
