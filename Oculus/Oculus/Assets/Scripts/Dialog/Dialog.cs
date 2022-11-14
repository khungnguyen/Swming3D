using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour, IDialog
{
    [SerializeField]
    private BoundInAndOut boundAnimation;

    [SerializeField]
    private TMP_Text tmpTitle;

    protected Action<object> okFunc;
    protected Action<object> cancelFunc;
    public virtual void Hide(Action onComplete = null, bool needDestroy = true)
    {
        boundAnimation.PlayBoundOutEffect(() =>
        {
            if (onComplete != null)
            {
                onComplete();
            }
            Utils.Log(this, "Hide", "Destroying object", needDestroy, gameObject.transform.name);
            if (needDestroy)
            {


                Destroy(gameObject);
            }

        });
    }

    public virtual Dialog Init(string title, Action<object> ok, Action<object> cancel)
    {
        if (tmpTitle != null)
        {
            tmpTitle.SetText(title);
        }
        okFunc = ok;
        cancelFunc = cancel;
        return this;
    }

    public virtual void Show(Action onComplete = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        boundAnimation.PlayBoundEffect(() =>
        {
            if (onComplete != null)
            {
                onComplete();
            }
        });
    }
    public virtual void Cancel()
    {

        Hide(() =>
        {
            if (cancelFunc != null)
            {
                cancelFunc(this);
            }
        });
    }
}
