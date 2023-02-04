using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BenInteractable : SnapInteraction
{
    List<SnapInteraction> interactionList;

    List<SnapInteraction> nolock;


    private SnapInteraction headInteraction;
    void Start()
    {
        var rootNode = transform.parent.parent.parent;
        interactionList = new List<SnapInteraction>(rootNode.GetComponentsInChildren<SnapInteraction>()).FindAll(e => e != this);
        nolock = interactionList.FindAll(e => e.lockTarget);
        headInteraction = interactionList.Find(e => e.transform.parent.parent.name == "Head");
    }
    public override void OnSelect()
    {
        if (enableWeight)
        {
            foreach (var mobject in nolock)
            {
                mobject.lockTarget = false;
            }
            if (headInteraction != null)
            {
                headInteraction.EnableInteraction(false);
            }
            foreach (var mobject in interactionList)
            {
                mobject.transform.parent.gameObject.SetActive(false);
            }
            base.OnSelect();//ooooo

        }

    }
    public override void OnUnselect()
    {
        if (enableWeight)
        {
            foreach (var mobject in interactionList)
            {
                mobject.transform.parent.gameObject.SetActive(true);
                mobject.EnableInteraction(true);
            }
            foreach (var mobject in nolock)
            {
                 mobject.lockTarget = true;
            }
            if (headInteraction != null)
            {
                headInteraction.EnableInteraction(true);
            }
            base.OnUnselect();
        }
    }
    IEnumerator EnableWeightEnumrator(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

    }
}
