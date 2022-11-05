using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class AnimateHandOVRCusomtize : AnimatedHandOVR
{
    /**
    * Force to disable thumb and pointin
    */
    protected override void UpdateCapTouchStates()
    {
        _isGivingThumbsUp = false;
        _isPointing = false;
    }
    /**
    * Change to hold for both grab and index button
    */
     protected override void UpdateAnimStates()
    {
        // Flex
        // blend between open hand and fully closed fist
        float flex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controller);
        float pinch = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, _controller);
        float choice = Mathf.Max(flex,pinch);
        _animator.SetFloat(_animParamIndexFlex, choice);
        // Point
        _animator.SetLayerWeight(_animLayerIndexPoint, _pointBlend);

        // Thumbs up
        _animator.SetLayerWeight(_animLayerIndexThumb, _thumbsUpBlend);

     
    }
}
