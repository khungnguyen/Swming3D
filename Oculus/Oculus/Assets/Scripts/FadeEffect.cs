using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private FadeEffectType _fadeType;
    [SerializeField] private float _fadeTime = 1;
    [SerializeField] private bool _autoStart = false;
    void Awake()
    {
        if (!_canvasGroup)
        {
            var hasComp = gameObject.GetComponent<CanvasGroup>();
            if (hasComp == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            else
                _canvasGroup = hasComp;
            if (_autoStart && FadeEffectType.FADE_IN == _fadeType)
            {
                _canvasGroup.alpha = 0;
            }
        }
    }
    void Start()
    {
        if (_autoStart)
        {
            StartFade();
        }
    }
    public void StartFade(Action onComplete = null)
    {
        StartFade(_fadeType, _fadeTime, onComplete);
    }
    public void StartFade(FadeEffectType t, Action onComplete = null)
    {
        StartFade(t, _fadeTime, onComplete);
    }
    public void StartFade(FadeEffectType t, float time, Action onComplete = null)
    {
        StartCoroutine(RunFade(t, time, onComplete));
    }
    private IEnumerator RunFade(FadeEffectType t, float maxTime, Action onComplete)
    {
        float timer = 0;
        float start = t == FadeEffectType.FADE_IN ? 0 : 1;
        float target = t == FadeEffectType.FADE_IN ? 1 : 0;
        while (timer < maxTime)
        {
            yield return null;
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(start, target, timer / maxTime);
        }
        onComplete?.Invoke();
    }
}
public enum FadeEffectType
{
    FADE_IN,
    FADE_OUT
}
