using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
public class KickBoardController : MonoBehaviourPunCallbacks, IInteraction
{
    public Transform boardHolderWrong;
    public Transform boardHolderRight;
    public Transform boardHolderSplashKick;

    public Transform fixedKickBoard; // animation kickboard, not for handling

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
    public void Activate(bool v)
    {
        gameObject.SetActive(v);
    }
    public void SnapTo(Transform to)
    {
        snapTo.setTarget(to);
    }
    public void SetBehavior(string trigger, string lastAnimation)
    {
        Utils.Log(this, "trigger", trigger);
        if (trigger == "StopAnim")
        {
            if (lastAnimation == "PositionWrong")
            {
                SnapTo(boardHolderWrong);

            }
            else if (lastAnimation == "SplashKickWrong")
            {
                SnapTo(boardHolderSplashKick);

            }
        }
        else if (trigger == "PositionWrong")
        {
            SnapTo(boardHolderWrong);

        }
        else if (trigger == "SplashKickWrong")
        {
            SnapTo(boardHolderSplashKick);

        }
        else if (trigger == "EnableKickBoard")
        {
            Activate(true);
        }
        else if (trigger == "Swim" || trigger == "Walk")
        {
            //  StartCoroutine(StopSwimInSecond(2));
            fixedKickBoard.gameObject.SetActive(true);
            Activate(false);
        }
        else
        {
            Activate(true);
            fixedKickBoard.gameObject.SetActive(false);
            SnapTo(boardHolderRight);
        }
    }
    public void HideKickBoards() {
        Activate(false);
        fixedKickBoard.gameObject.SetActive(false);
    }
}
