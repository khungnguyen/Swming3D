using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class BodySnapInteraction : SnapInteraction
{
    private bool onSelect = false; 

    public Transform wholeBody;

    public Transform kickBoard;

    public Transform newParentOfKickBoard;

    private Transform curentKicboardParent;

    void Awake() {
        if(!VRAppDebug.USE_BODY_MOVING) {
            gameObject.SetActive(false);
        }
        base.Awake();
    }
    public override void OnSelect()
    {
        curentKicboardParent = kickBoard.parent;
        kickBoard.parent = newParentOfKickBoard;
        onSelect = true;
        Debug.LogError("BodySnapInteraction OnSelect");

    }
    public override void OnUnselect()
    {
        onSelect = false;
        Debug.LogError("BodySnapInteraction OnUnselect");
        // vfdjfkdfs
        kickBoard.parent = curentKicboardParent;
    }
    void Update() {

    }
    void FixedUpdate() {
        if(onSelect) {
            Vector3 positionOffset = wholeBody.position - snapTo.position;

            Vector3 rotaionOffset = wholeBody.eulerAngles - snapTo.eulerAngles;
            wholeBody.SetPositionAndRotation(transform.position + positionOffset,transform.rotation);
            
        }
        else {
           transform.SetPositionAndRotation(snapTo.position,wholeBody.rotation);
        }
    }

}
