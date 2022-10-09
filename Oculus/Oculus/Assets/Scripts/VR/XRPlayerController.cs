using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class XRPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject trackkingSpace;


    //[SerializeField]
    //private ContinousMove locomotion;

    void Start()
    {
        if (!DectectVR.instancne.isVR)
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<OVRPlayerController>().enabled = false;
            GetComponentInChildren<OVRManager>().enabled = false;
            GetComponentInChildren<OVRCameraRig>().enabled = false;
            trackkingSpace.SetActive(false);
        }
        else
        {
            Debug.Log("Using Camera of XR");
        }

    }

}
