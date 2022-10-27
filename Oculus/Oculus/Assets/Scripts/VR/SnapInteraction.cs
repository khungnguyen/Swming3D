using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System;
using Photon.Pun;
using System.Collections;

public class SnapInteraction : MonoBehaviourPunCallbacks, IPointableElement, IInteraction
{

    const string TAG = "[SnapInteraction]";
    public MonoBehaviour constraintComp;

    public Transform snapTo;

    public GameObject visualGameObject;

    public bool lockTarget = false;

    private bool isSnap = true;

    public bool disableSnapFunct = false;

    public bool enableWeight;

    public bool lockRotation;

    public event Action<PointerEvent> WhenPointerEventRaised;


    private Vector3 orginalRotation;
    public void Awake()
    {
        bool usePhotonView = !(photonView != null && photonView.IsMine);
        disableSnapFunct = false;

            if (constraintComp == null || (!(constraintComp is TwoBoneIKConstraint) && !(constraintComp is MultiAimConstraint)))
            {
                constraintComp = transform.parent.GetComponent<TwoBoneIKConstraint>();
                if (constraintComp == null)
                {
                    constraintComp = transform.parent.GetComponent<MultiAimConstraint>();
                }
            }
            EnableRigWeight(false, true);
    }

    IEnumerator EnableWeightEnumrator(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        EnableRigWeight();
    }
    public void EnableRigWeight(bool enable = true, bool force = false)
    {
       // Debug.LogError(TAG + "EnableRigWeight" + enable + "disableSnapFunct" + disableSnapFunct);
        enableWeight = enable;
        if (!disableSnapFunct || force)
        {
            if (constraintComp is TwoBoneIKConstraint)
            {
                ((TwoBoneIKConstraint)constraintComp).weight = enable ? 1 : 0;
            }
            else if (constraintComp is MultiAimConstraint)
            {
                ((MultiAimConstraint)constraintComp).weight = enable ? 1 : 0;
            }
        }
        EnableGrabBableCube(enable);
    }

    private void GetOriginalRoation()
    {
        orginalRotation = snapTo.rotation.eulerAngles;
    }
    //dssd
    void Start()
    {
        // StartCoroutine(EnableWeightEnumrator(2));
    }
    public void ProcessPointerEvent(PointerEvent evt)
    {
        if (!disableSnapFunct)
        {
            if (photonView.IsMine)
            {
                switch (evt.Type)
                {
                    case PointerEventType.Select:
                        OnSelect();
                        break;
                    case PointerEventType.Unselect:
                        OnUnselect();
                        break;
                    case PointerEventType.Move:
                        break;
                }
            }
        }


    }
    public void OnSelect()
    {
        isSnap = false;
        GetOriginalRoation();
        // activateConstraintComp(true);
    }
    public void OnUnselect()
    {
        isSnap = true;
        SnapUpdate();
        // activateConstraintComp(false);
    }

    /*
     * lockTarget = true so alway snap to target, target move, cube moves
     * lockTarget = false : cube will be static and target was locked
     */
    public void Update()
    {
        if (!disableSnapFunct)
        {

            if (isSnap)
            {
                if (lockTarget && enableWeight)
                {
                    // do nothing
                }
                else
                {
                    SnapUpdate();
                }
            }
        }
        if (lockRotation && !isSnap)
        {
            transform.rotation = Quaternion.Euler(orginalRotation);
        }
    }
    private void SnapUpdate()
    {
        if (Vector3.Distance(transform.position, snapTo.position) >= 0.01)
        {
            transform.position = snapTo.position;
        }
        if (Vector3.Distance(transform.rotation.eulerAngles, snapTo.rotation.eulerAngles) >= 0.01)
        {
            transform.rotation = snapTo.rotation;
        }
    }
    private void activateConstraintComp(bool active)
    {
        if (constraintComp is TwoBoneIKConstraint)
        {
            ((TwoBoneIKConstraint)constraintComp).enabled = active;
        }
        else if (constraintComp is MultiAimConstraint)
        {
            ((MultiAimConstraint)constraintComp).enabled = active;
        }
    }
    public void EnableGrabBableCube(bool a)
    {
        visualGameObject.GetComponent<MeshRenderer>().enabled = a;
    }
    public override void OnDisable()
    {

    }
    public void EnableInteraction(bool enable)
    {
        EnableRigWeight(enable);
    }

}
