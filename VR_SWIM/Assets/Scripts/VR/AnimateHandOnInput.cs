using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue =  pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger",triggerValue);
       float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}
