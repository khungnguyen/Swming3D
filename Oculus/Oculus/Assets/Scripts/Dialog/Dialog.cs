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

    [SerializeField]
    private TMP_Text tmpDescription;

    [SerializeField]
    private Transform buttonLayout;

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

    public virtual Dialog Init(DialogOption option, Action<object> okFunc, Action<object> cancelFunc)
    {
        if (tmpTitle != null)
        {
            tmpTitle.SetText(option.title);
        }
        if (tmpDescription != null)
        {
            if (option.HasDescription())
            {
                tmpDescription.SetText(option.description);
            }
            else
            {
                tmpDescription.gameObject.SetActive(false);
            }

        }
        buttonLayout.gameObject.SetActive(!option.hideButton);
        this.okFunc = okFunc;
        this.cancelFunc = cancelFunc;
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
    public virtual void Ok()
    {
        Hide(() =>
       {
           if (okFunc != null)
           {
               okFunc(this);
           }
       });
    }
}
