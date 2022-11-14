using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundInAndOut : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxBoundSize = 1.1f;
    public float minBoundSize = 0.9f;
    public float startBound = 0f;
    public float originalBound = 1f;

    public Transform mTransform;
    void Awake()
    {
        if (mTransform == null)
        {
            mTransform = transform;
        }
    }
    public void PlayBoundEffect(Action OnCompleted = null)
    {
        Reset();
        StartCoroutine(BoundInnOut(OnCompleted));
    }

    /**
    *Bound In and Out : Zoom in then Zoom out then be normal size
    */
    IEnumerator BoundInnOut(Action OnCompleted)
    {
        yield return StartCoroutine(Bound(startBound, maxBoundSize, 0.2f));
        yield return StartCoroutine(Bound(maxBoundSize, minBoundSize, 0.1f));
        yield return StartCoroutine(Bound(minBoundSize, originalBound, 0.1f));
        if (OnCompleted != null)
        {
            OnCompleted();
        }
    }

    /**
    * Bound function to scale a transfrom from start value to target value by time
    * @param start : begin scale value, would be converted to Vector3
    * @param target : end scale value, would be converted to Vector3
    * @param maxTime : How long scale effect take.
    */
    IEnumerator Bound(float start, float target, float maxTime)
    {
        float timer = 0;
        Vector3 startVector = Vector3.one * start;
        Vector3 tarrgetVector = Vector3.one * target;
        while (timer < maxTime)
        {
            yield return null;
            timer += Time.deltaTime;
            mTransform.localScale = Vector3.Lerp(startVector, tarrgetVector, timer / maxTime);
        }
    }
    public void Reset()
    {
        StopAllCoroutines();
        mTransform.localScale = Vector3.zero;
    }
    /**
    *Bound In and Out : Zoom in then Zoom out then be normal size
    */
    IEnumerator BoundOut(Action OnCompleted)
    {
        yield return StartCoroutine(Bound(originalBound, startBound, 0.1f));
        if (OnCompleted != null)
        {
            OnCompleted();
        }
    }
    public void PlayBoundOutEffect(Action OnCompleted = null)
    {
        StartCoroutine(BoundOut(OnCompleted));
    }
}
