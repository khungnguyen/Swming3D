using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MasterClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // if(PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) {
        //     Transform point = SpawnManager.instance.GetSpawnTranform();
        //     PhotonNetwork.Instantiate("Student",Vector3.zero,point.transform.rotation);
        //     Debug.Log("Creating Aniamtion");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
