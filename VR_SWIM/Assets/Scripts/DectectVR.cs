using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class DectectVR : MonoBehaviour
{
    public GameObject xrOrigin;
    public GameObject desktopCharacter;
    public GameObject MockHMD;

    void Start()
    {
        var xrSetting = XRGeneralSettings.Instance;
        if (xrSetting == null)
        {
            Debug.Log("XRGeneral Setting is null");
            return;
        }
        var xrManager = xrSetting.Manager;
        if (xrManager == null)
        {
            Debug.Log("XRGeneral Setting is null");
            return;
        }
        var xrLoader = xrManager.activeLoader;
        if (xrLoader != null)
        {
            Debug.Log("Device detected" + xrLoader.name);
            if(xrLoader.name.IndexOf("Mock HMD")!=-1)
            {
                Debug.Log("Using FAKE controller - MOCK HMD");
                MockHMD.SetActive(true);
            }
            else
            {
                MockHMD.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No VR Device detect");
        }

        xrOrigin.SetActive(xrLoader != null);
        desktopCharacter.SetActive(xrLoader == null);

    }

}
