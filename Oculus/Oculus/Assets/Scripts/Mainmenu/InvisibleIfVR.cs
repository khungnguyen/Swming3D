using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleIfVR : MonoBehaviour
{
    /*
    * Invisible GameObject of current Owner is VR player
    */
    void Start()
    {

        gameObject.SetActive(!DectectVR.instancne.isVR);
    }

}
