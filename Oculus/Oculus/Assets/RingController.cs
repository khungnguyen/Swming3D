using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    [SerializeField] Transform targetSnapTo;
    [SerializeField] Transform actualRing;
    [SerializeField] Transform signToPutRing;
    [SerializeField] RingCollisonDetecter collisionDetecter;

    private bool onCollision = false;
    public void OnSelect()
    {

    }

    public void OnUnselect()
    {
        if(onCollision) {
            transform.SetPositionAndRotation(targetSnapTo.position,targetSnapTo.rotation);
            actualRing.gameObject.SetActive(true);
            signToPutRing.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void CollisionDectectEvent(Transform t,bool onEnter) {
        Utils.Log(this, "Collider",t.name,onEnter);
        onCollision = onEnter;
    }
    void OnEnable() {
        collisionDetecter.listener+=CollisionDectectEvent;
    }
    void OnDisable() {
        collisionDetecter.listener-=CollisionDectectEvent;
    }
}
