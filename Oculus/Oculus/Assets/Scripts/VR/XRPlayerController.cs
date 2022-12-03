using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;


public class XRPlayerController : MonoBehaviour
{
    /*
    * Handle Disable/enable Camera Rig
    * Adding input interaction
    */
    [SerializeField]
    private GameObject trackkingSpace;

    [SerializeField]
    private Transform cameraOffset;


    [SerializeField]
    private Transform leftController;

    [SerializeField]
    private Transform rightController;


    public float speedY = 0.5f;

    const float MAX_Y = 1f;

    const float MIN_Y = -2f;
    private Vector3 curCameraPos;
    void Start()
    {
        if (!DectectVR.instancne.isVR)
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<OVRPlayerController>().enabled = false;
            GetComponentInChildren<OVRManager>().enabled = false;
            GetComponentInChildren<OVRCameraRig>().enabled = false;
            trackkingSpace.SetActive(false);
            leftController.GetComponentInChildren<HandVisualCustomize>().ForceVisibleHand(true);
            rightController.GetComponentInChildren<HandVisualCustomize>().ForceVisibleHand(true);
        }
        else
        {
            Debug.Log("Using Camera of XR");
        }

    }

    public void Update()
    {
        if (DectectVR.instancne.isVR)
        {
            Input();
        }

    }
    /** 
    * Handle Input of button 
    * A - X to move camera up down
    * B - Y to move camera up up
    */
    private void Input()
    {
        if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three))
        {
           // Debug.Log("Button One Three Press");
            curCameraPos = cameraOffset.transform.position;
            curCameraPos.y -= speedY * Time.deltaTime;
            curCameraPos.y =Mathf.Clamp( curCameraPos.y,MIN_Y,MAX_Y);
            cameraOffset.transform.position = curCameraPos;
        }
        else if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
           // Debug.Log("Button One Two Four Press");
            curCameraPos = cameraOffset.transform.position;
            curCameraPos.y += speedY * Time.deltaTime;
            curCameraPos.y =Mathf.Clamp(curCameraPos.y,MIN_Y,MAX_Y);
            cameraOffset.transform.position = curCameraPos;
        }
    }
}
