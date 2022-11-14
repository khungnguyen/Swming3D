
using System;

public interface IDialog 
{
   public void Show(Action onComplete);
   public void Hide(Action onComplete,bool needDestroy);
   public Dialog Init(string title,Action<object> ok,Action<object> cancel) ;
}
