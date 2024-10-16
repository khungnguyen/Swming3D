using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RingController : MonoBehaviour
{
    [SerializeField] Transform targetSnapTo;
    [SerializeField] Transform actualRing;
    [SerializeField] Transform signToPutRing;
    [SerializeField] RingCollisonDetecter collisionDetecter;

    [SerializeField] Transform parent;
    [SerializeField] Transform ringLayer;
    [SerializeField] GameObject bodyMoving;

    private Vector3 parentPosition;
    private Quaternion quaternion;
    void Start() {
       parentPosition = parent.position;
       quaternion= parent.rotation;
       ringLayer.parent = parent.parent; 
    }
    private bool onCollision = false;
    public void OnSelect()
    {

    }

    public void OnUnselect()
    {
        if (onCollision)
        {
            gameObject.GetPhotonView().RPC("PutTheRing",RpcTarget.All);
        }
    }
    [PunRPC]
    public void PutTheRing()
    {
        transform.SetPositionAndRotation(targetSnapTo.position, targetSnapTo.rotation);
        actualRing.gameObject.SetActive(true);
        signToPutRing.gameObject.SetActive(false);
        gameObject.SetActive(false);
        parent.SetPositionAndRotation(parentPosition,quaternion);
        Destroy(bodyMoving);
    }

    void CollisionDectectEvent(Transform t, bool onEnter)
    {
        Utils.Log(this, "Collider", t.name, onEnter);
        onCollision = onEnter;
        // if(onEnter)
        // PutTheRing();

    }
    void OnEnable()
    {
        collisionDetecter.listener += CollisionDectectEvent;
    }
    void OnDisable()
    {
        collisionDetecter.listener -= CollisionDectectEvent;
    }
}
