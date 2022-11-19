using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAppDebug
{
    public static readonly bool USE_DEBUG_VR_SINGLE_PREVIEW = false;

    public static readonly bool USE_BODY_MOVING = false; 

    public static readonly bool USE_NEW_PHOTON_ROOM_DESIGN = true; 

    public static readonly bool USE_NEW_MENU_DESIGN = false; 

    /**
    * ONLY CHOOSE ONE
    */
    public static readonly bool USE_AUTO_DETECT_VR = false;

    public static readonly bool FORCE_USE_VR = !USE_AUTO_DETECT_VR && false;

    public static readonly bool FORCE_USE_NO_VR =  !USE_AUTO_DETECT_VR && false;
    // Disable interaction of body
}
