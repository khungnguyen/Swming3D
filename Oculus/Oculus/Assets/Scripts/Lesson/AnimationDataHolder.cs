using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnimationDataItem
{
    public RuntimeAnimatorController controller;
    public Avatar avatar;
}
public class AnimationDataHolder : MonoBehaviour
{
    public static AnimationDataHolder instance;
    public void Awake()
    {
        instance = this;
    }
    public List<AnimationDataItem> animationControllers;

    public AnimationDataItem GetAnimationDataByControllerName(string name)
    {
        return animationControllers.Find(e => e.controller.name == name);
    }
}
