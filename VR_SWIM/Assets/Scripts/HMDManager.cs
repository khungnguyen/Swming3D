using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Device");
        }
        else if (XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "Mock HMD Loader")
        {
            Debug.Log("Using MOCK HMD LOADER");
        }
        else
        {
            Debug.Log("Using" + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
