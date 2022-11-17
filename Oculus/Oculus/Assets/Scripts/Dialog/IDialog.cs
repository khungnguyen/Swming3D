
using System;

public interface IDialog 
{
   public void Show(Action onComplete);
   public void Hide(Action onComplete,bool needDestroy);
   public Dialog Init(DialogOption option,Action<object> ok,Action<object> cancel) ;
}
public struct DialogOption
{
    public bool hideButton;

    public string title;

    public string description;

    public bool HasDescription() {
      return description!=null && description.Length>0;
    }
}
public enum DialogType {
  DialogScrollBase,
  DialogNoticeBase,
  DialogScrollRooms,
  DialogExersiesAction,
  DialogScrollMenu
}