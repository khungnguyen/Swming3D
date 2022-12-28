using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BackFloat : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
       Utils.LogError(this,"BackFloat Trigger Collider Enter");
       gameObject.GetPhotonView().RPC("InActiveItself",RpcTarget.All);
    }
    [PunRPC]
    public void InActiveItself() {
       gameObject.SetActive(false);
    }
     private void OnTriggerExit(Collider collider)
    {
       Utils.LogError(this,"BackFloat Trigger Collider Exit");
    }
}
