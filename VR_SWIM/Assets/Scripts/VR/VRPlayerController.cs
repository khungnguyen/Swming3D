using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private XROrigin xrOrigin;

    [SerializeField]
    private LocomotionSystem locomotion;

    //[SerializeField]
    //private ContinousMove locomotion;

    void Start()
    {
        if (!DectectVR.instancne.isVR)
        {
            xrOrigin.enabled = false;
            locomotion.enabled = false;
            camera.transform.gameObject.SetActive(false);
            Debug.Log("Disable Camera of XR");
        }
        else
        {
            Debug.Log("Using Camera of XR");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
