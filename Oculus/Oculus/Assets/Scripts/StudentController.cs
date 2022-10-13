using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StudentController : MonoBehaviourPunCallbacks, IReciever
{
    public Animator animator;
    public Avatar animatorAvatar;
    public RigBuilder rig;
    public void OnActionReciver(EventCodes theEvent, object[] packages)
    {

        switch(theEvent)
        {
            case EventCodes.ActionPlayAnimation:
                rig.enabled = false;
                string animation = (string)packages[1];
                animator.avatar = animatorAvatar;
                animator.SetBool(animation, true);
                break;
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        //if()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);
    }
    public void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
    }
}
