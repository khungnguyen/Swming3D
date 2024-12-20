using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Photon.Pun;
using UnityEngine;

public class BodySnapInteraction : SnapInteraction
{
    private bool onSelect = false;

    private PhotonTransformView kickBoardPhotonView;

    public Transform wholeBody;

    public Transform kickBoard;

    private Transform newParentOfKickBoard;

    private Transform curentKicboardParent;

    void Awake()
    {
        // if (!VRAppDebug.USE_BODY_MOVING)
        // {
        //     gameObject.SetActive(false);
        //     Utils.LogError("Disable body moving");
        // }
        base.Awake();
        kickBoardPhotonView = GetComponent<PhotonTransformView>();
        newParentOfKickBoard = wholeBody;
    }
    void Start()
    {
        newParentOfKickBoard = transform.parent;
    }
    public override void OnSelect()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ChangeParent", RpcTarget.All);
            onSelect = true;
            Debug.LogError("BodySnapInteraction OnSelect");
        }
    }
    public override void OnUnselect()
    {
        if (photonView.IsMine)
        {
            onSelect = false;
            Debug.LogError("BodySnapInteraction OnUnselect");
            photonView.RPC("ResetParent", RpcTarget.All);

        }

    }
    [PunRPC]
    public void ChangeParent()
    {
        // Fix kickboard unexpected movement when changing parent of unowner
        if (!photonView.IsMine)
        {
            StartCoroutine(DelayActivePhotonViewKickBoard());
        }
        kickBoard.GetComponent<SnapTo>().setTarget(null);
        curentKicboardParent = kickBoard.parent;
        kickBoard.parent = newParentOfKickBoard.parent;
    }
    private IEnumerator DelayActivePhotonViewKickBoard()
    {
        kickBoardPhotonView.enabled = false;
        yield return new WaitForSeconds(0.5f);
        kickBoardPhotonView.enabled = true;
    }
    [PunRPC]
    public void ResetParent()
    {
         if (!photonView.IsMine)
        {
            StartCoroutine(DelayActivePhotonViewKickBoard());
        }
        kickBoard.parent = curentKicboardParent;
        // Fix kickboard unexpected movement when changing parent of unowner
       
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            if (onSelect)
            {
                Vector3 positionOffset = wholeBody.position - snapTo.position;
                wholeBody.SetPositionAndRotation(transform.position + positionOffset, transform.rotation);

            }
            else
            {
                transform.SetPositionAndRotation(snapTo.position, wholeBody.rotation);
            }
        }
    }

}
