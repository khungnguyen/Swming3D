using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
public class KickBoardController : MonoBehaviourPunCallbacks, IInteraction
{
    /**
    * Handle interaction of Kickboard
    * Try to disable snap to when being interacted by VR player
    */
    enum MATERIAL_USE
    {
        NORMAL = 0,
        ON_INTERACTION,
        ON_GRAB
    }
    const string TAG = "[KickBoardController]";

    public MeshRenderer kickBoardMaterial;
    public List<Material> materials = new List<Material>();

    /**

    */

    public SnapTo snapTo;
    /**
    * Set target null : disable snap to
    */

    private PhotonTransformView photonTransformView;
    void Start()
    {
    }
    public void OnSelect()
    {
        snapTo.setTarget(null);
        ChangMaterial(MATERIAL_USE.ON_GRAB);

    }

    public void OnUnselect()
    {
        ChangMaterial(MATERIAL_USE.ON_INTERACTION);
    }
    public void EnableInteraction(bool enable)
    {
        if (kickBoardMaterial)
        {
            ChangMaterial(enable ? MATERIAL_USE.ON_INTERACTION : MATERIAL_USE.NORMAL);
        }
        else
        {
            Debug.Log(TAG + "Missing data kickBoardMaterial");
        }

    }
    private void ChangMaterial(MATERIAL_USE type)
    {
        Material mat = materials[(int)type]; ;
        if (mat != null)
        {
            kickBoardMaterial.material = mat;
        }
    }

}
