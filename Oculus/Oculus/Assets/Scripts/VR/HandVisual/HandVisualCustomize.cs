using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class HandVisualCustomize : HandVisual
{

    const string TAG = "[HandVisualCustomize]";
    private bool _IsVisible;

    private bool forceVisibleHand = false;

    public void ForceVisibleHand(bool x)
    {
        forceVisibleHand = x;
    }
    public void UpdateSkeleton()
    {
       
        if (forceVisibleHand)
        {
            _skinnedMeshRenderer.enabled = true;
        }
        else
        {
            base.UpdateSkeleton();
        }
    }
    protected virtual void OnEnable()
    {
        if (_started)
        {
            Hand.WhenHandUpdated += UpdateSkeleton;
        }
    }

    protected virtual void OnDisable()
    {
        if (_started && _hand != null)
        {
            Hand.WhenHandUpdated -= UpdateSkeleton;
        }
    }

}
