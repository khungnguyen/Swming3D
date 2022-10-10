using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class DectectVR : MonoBehaviour
{
    public bool noVR;
    public bool forceVR = false;
    public GameObject[] desktopCharacter;
    public GameObject[] vrGameObject;
    public static DectectVR instancne;
    public bool isVR;
    private void Awake()
    {
        instancne = this;
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
            Debug.Log("Device detected " + xrLoader.name);

        }
        else
        {
            Debug.Log("No VR Device detect");
        }
        
        if(forceVR && noVR) {
            Debug.LogError("ForceVR = true so noVR = false");
            noVR = false;
            forceVR = true;
        } 
        isVR = (xrLoader != null && !noVR) || forceVR;
        foreach(var ob  in vrGameObject)
        {
            ob.SetActive(isVR);
        }
        foreach (var ob  in desktopCharacter)
        {
            ob.SetActive(!isVR);
        }
    }
    void Start()
    {
        
    }

}
