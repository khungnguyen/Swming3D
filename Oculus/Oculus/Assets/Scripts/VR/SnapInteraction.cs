using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System;

public class SnapInteraction : MonoBehaviour, IPointableElement
{
    // Start is called before the first frame update

    public MonoBehaviour constraintComp;
    public Transform snapTo;

    private bool isSnap = true;

    public event Action<PointerEvent> WhenPointerEventRaised;

    void Start()
    {
        // Find themself in parent
        if (constraintComp == null || (!(constraintComp is  TwoBoneIKConstraint) && !(constraintComp is  MultiAimConstraint)))
        {
            constraintComp = transform.parent.GetComponent<TwoBoneIKConstraint>();
            if (constraintComp == null)
            {
                constraintComp = transform.parent.GetComponent<MultiAimConstraint>();
            }
        }
        activateConstraintComp(false);
    }
    public void ProcessPointerEvent(PointerEvent evt)
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
    public void OnSelect()
    {
        isSnap = false;
        activateConstraintComp(true);
        Debug.LogError("OnSelect");

    }
    public void OnUnselect()
    {
        isSnap = true;
        transform.position = snapTo.position;
        transform.rotation = snapTo.rotation;
        activateConstraintComp(false);
    }
    public void Update()
    {
        if (isSnap)
        {
            //transform.position = snapTo.position;
            //transform.rotation = snapTo.rotation;
        }
    }
    private void activateConstraintComp(bool active)
    {
        if (constraintComp is TwoBoneIKConstraint)
        {
           // ((TwoBoneIKConstraint)constraintComp).enabled = active;
           // Debug.LogError("((TwoBoneIKConstraint)constraintComp).weight" + ((TwoBoneIKConstraint)constraintComp).weight);
        }
        else if (constraintComp is MultiAimConstraint)
        {
           // ((MultiAimConstraint)constraintComp).enabled = active;
        }
    }
    public void visible(bool a)
    {
        transform.gameObject.SetActive(a);
    }

}
