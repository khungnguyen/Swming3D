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
        Utils.LogError(this,"Root name",rootNode.name);
        interactionList = new List<SnapInteraction>(rootNode.GetComponentsInChildren<SnapInteraction>()).FindAll(e => e != this);
        nolock = interactionList.FindAll(e => e.lockTarget);
        headInteraction = interactionList.Find(e => e.transform.parent.parent.name == "Head");
        Utils.LogError(this,"RheadInteraction",headInteraction.transform.name);
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
                if(mobject.transform.name == "BodyMoving") {
                    mobject.gameObject.SetActive(false);
                }
                else {
                    if(mobject.transform.parent.parent.TryGetComponent<Rig>(out var rig)) {
                        rig.gameObject.SetActive(false);
                        
                    }
                }  
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
               if(mobject.transform.name == "BodyMoving") {
                    mobject.gameObject.SetActive(true);
                }
                else {
                    if(mobject.transform.parent.parent.TryGetComponent<Rig>(out var rig)) {
                        rig.gameObject.SetActive(true);
                        mobject.EnableInteraction(true);
                    }
                }  
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
