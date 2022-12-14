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

    public bool isSnap = true;

    public bool disableSnapFunct = false;

    public bool enableWeight;

    public bool lockRotation;

    public bool lockMovement = false;

    public event Action<PointerEvent> WhenPointerEventRaised;


    private Vector3 originalRotation;
    private Vector3 originalPosition;
    public void Awake()
    {
        disableSnapFunct = false;

        if (constraintComp == null || (!(constraintComp is TwoBoneIKConstraint) && !(constraintComp is MultiAimConstraint) && !(constraintComp is MultiRotationConstraint)))
        {
            constraintComp = transform.parent.GetComponent<TwoBoneIKConstraint>();
            if (constraintComp == null)
            {
                constraintComp = transform.parent.GetComponent<MultiAimConstraint>();
            }
            if (constraintComp == null)
            {
                constraintComp = transform.parent.GetComponent<MultiRotationConstraint>();
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
        enableWeight = enable;
        if (enable) // update posistion before enable rig weight
        {
            SnapUpdate();
        }
        if (!disableSnapFunct || force)
        {
            if (constraintComp is TwoBoneIKConstraint ikbone)
            {
                ikbone.weight = enable ? 1 : 0;
            }
            else if (constraintComp is MultiAimConstraint aim)
            {
                aim.weight = enable ? 1 : 0;
            }
            else if (constraintComp is MultiRotationConstraint rotation)
            {
                rotation.weight = enable ? 1 : 0;
            }
        }

        EnableGrabBableCube(enable);
         if (enable) // update posistion before enable rig weight
        {
            SnapUpdate();
        }
    }

    private void SetOriginProperties()
    {
        originalRotation = snapTo.rotation.eulerAngles;
        originalPosition = snapTo.position;
    }
    //dssd
    void Start()
    {

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
    public virtual void OnSelect()
    {
        isSnap = false;
        SetOriginProperties();
    }
    public virtual void OnUnselect()
    {
        SnapUpdate();
        SetOriginProperties();
        isSnap = true;
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
            else
            {
                if (lockRotation)
                {
                    transform.rotation = Quaternion.Euler(originalRotation);
                }

            }
            if (lockMovement)
            {
                transform.position = snapTo.position;
            }
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
