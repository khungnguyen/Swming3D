using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;

public class SnapInteraction : Grabbable
{
    // Start is called before the first frame update

    public MonoBehaviour constraintComp;
    public Transform snapTo;

    private bool isSnap = true;
    protected override void Start()
    {
        // Find themself in parent
        if (constraintComp == null || (constraintComp is not TwoBoneIKConstraint && constraintComp is not MultiAimConstraint))
        {
            constraintComp = transform.parent.GetComponent<TwoBoneIKConstraint>();
            if (constraintComp == null)
            {
                constraintComp = transform.parent.GetComponent<MultiAimConstraint>();
            }
        }
        activateConstraintComp(false);
        base.Start();
    }
    public override void ProcessPointerEvent(PointerEvent evt)
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
        base.ProcessPointerEvent(evt);
    }
    public void OnSelect()
    {
        isSnap = false;
        activateConstraintComp(true);

    }
    public void OnUnselect()
    {
        isSnap = true;
        activateConstraintComp(false);
        transform.position = snapTo.position;
        transform.rotation = snapTo.rotation;
    }
    public void Update()
    {
        if (isSnap)
        {
            transform.position = snapTo.position;
            transform.rotation = snapTo.rotation;
        }
    }
    private void activateConstraintComp(bool active)
    {
        if (constraintComp is TwoBoneIKConstraint)
        {
            ((TwoBoneIKConstraint)constraintComp).weight = active ? 1 : 0;
        }
        else if (constraintComp is MultiAimConstraint)
        {
            ((MultiAimConstraint)constraintComp).weight = active ? 1 : 0;
        }
    }
    public void visible(bool a)
    {
        transform.gameObject.SetActive(a);
    }

}
