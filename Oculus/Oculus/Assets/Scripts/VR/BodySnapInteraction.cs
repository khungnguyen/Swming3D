using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class BodySnapInteraction : SnapInteraction
{
    private bool onSelect = false; 

    public Transform wholeBody;
    public override void OnSelect()
    {
        onSelect = true;
        Debug.LogError("BodySnapInteraction OnSelect");

    }
    public override void OnUnselect()
    {
        onSelect = false;
        Debug.LogError("BodySnapInteraction OnUnselect");
        // vfdjfkdfs
    }
    void Update() {

    }
    void FixedUpdate() {
        if(onSelect) {
            Vector3 positionOffset = wholeBody.position - snapTo.position;

            Vector3 rotaionOffset = wholeBody.eulerAngles - snapTo.eulerAngles;
           // Vector3 ve = transform.position - lastPostion;
            wholeBody.SetPositionAndRotation(transform.position + positionOffset,transform.rotation);
            
        }
        else {
           transform.SetPositionAndRotation(snapTo.position,wholeBody.rotation);
        }
    }

}
