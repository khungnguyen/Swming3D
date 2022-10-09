using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera camera;


    //[SerializeField]
    //private ContinousMove locomotion;

    void Start()
    {
        if (!DectectVR.instancne.isVR)
        {
            //camera.transform.gameObject.SetActive(false);
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
