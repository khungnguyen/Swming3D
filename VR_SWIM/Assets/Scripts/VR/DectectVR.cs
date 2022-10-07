using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class DectectVR : MonoBehaviour
{
    public bool noVR;
    public GameObject xrOrigin;
    public GameObject desktopCharacter;
    public GameObject MockHMD;
    public static DectectVR instancne;
    public bool isVR;
    private void Awake()
    {
        instancne = this;
    }
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
        isVR = xrLoader != null && !noVR;
        //xrOrigin.SetActive(isVR);
        desktopCharacter.SetActive(!isVR);
    }

}
