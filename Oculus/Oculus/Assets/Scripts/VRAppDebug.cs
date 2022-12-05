using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAppDebug
{
    public static readonly bool USE_DEBUG_VR_SINGLE_PREVIEW = false;

    /**
    * USE_DEBUG_NO_VR_SINGLE_PREVIEW : Instantiate student without using VR to checking lesson
    */
    public static readonly bool USE_DEBUG_NO_VR_SINGLE_PREVIEW = false;

    public static readonly bool USE_BODY_MOVING = false;

    /**
        Using new menu design for Lobby, room in main menu
    */
    public static readonly bool USE_NEW_PHOTON_ROOM_DESIGN = false;

    /**
    Using new menu design for Examiner
    */
    public static readonly bool USE_NEW_MENU_DESIGN = false;

    /**
    * USE FOR BUILDING APPLICATION
    * USE_AUTO_DETECT_VR = true : Auto detect VR device no matter what you tried to force using vr or novr
    * FORCE_USE_VR : only work when USE_AUTO_DETECT_VR = false, forcing VR mode
    * FORCE_USE_NO_VR : only work when USE_AUTO_DETECT_VR = false, forcing No VR mode
    */
    public static readonly bool USE_AUTO_DETECT_VR = true;

    public static readonly bool FORCE_USE_VR = !USE_AUTO_DETECT_VR && false;

    public static readonly bool FORCE_USE_NO_VR = !USE_AUTO_DETECT_VR && true;
    // Disable interaction of body
}
