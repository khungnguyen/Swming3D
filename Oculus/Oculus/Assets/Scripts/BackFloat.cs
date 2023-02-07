using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BackFloat : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public Action OnInteraction;
    private void OnTriggerEnter(Collider collider)
    {
        if (gameObject.activeSelf)
        {
            //Utils.LogError(this, "BackFloat Trigger Collider Enter");
            gameObject.GetPhotonView().RPC("InActiveItself", RpcTarget.All);
        }

    }
    private void OnTriggerExit(Collider collider)
    {
       // Utils.LogError(this, "BackFloat Trigger Collider Exit");
    }
    [PunRPC]
    public void InActiveItself()
    {
        gameObject.SetActive(false);
        BackFloatFlashingOff();
        OnInteraction?.Invoke();
    }
    public void BackFloatFlashing()
    {
        if (gameObject.activeSelf)
        {
            animator.SetTrigger("ActiveChangeColor");
        }
    }
    public void BackFloatFlashingOff()
    {
        animator.ResetTrigger("ActiveChangeColor");
    }
}
