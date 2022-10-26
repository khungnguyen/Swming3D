using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System;
using Photon.Pun;
using System.Collections;
public class KickBoardController :  MonoBehaviourPunCallbacks
{
    /**
    * Handle interaction of Kickboard
    * Try to disable snap to when being interacted by VR player
    */
    const string TAG = "[KickBoardController]";


   


    public SnapTo snapTo;
    /**
    * Set target null : disable snap to
    */
    public void OnSelect()
    {
        snapTo.setTarget(null);
    }
    public void OnUnselect()
    {
        //quy nguyen
    }
   
}
