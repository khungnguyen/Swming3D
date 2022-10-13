using Photon.Pun;
using UnityEngine;

public class ExminerDecision : MonoBehaviour
{
    enum DIALOG
    {
        CONFIRM,
        PLAY
    }
    public GameObject panel;
    public GameObject confirmDialog;
    public GameObject playDialog;
    // Start is called before the first frame update
    private GameObject curDialog;
    private LessionUnit curLesson;
    void Start()
    {
        ActivateDialog(playDialog, false);
        curDialog = confirmDialog;
        curLesson = LessonManager.instance.GetCurLesson();
    }

    public void YesClick()
    {
        sendActionYes();
        SwitchDialog(DIALOG.PLAY);
    }
    public void NoClick()
    {
        sendActionNo();
    }
    public void PlayClick()
    {
        SwitchDialog(DIALOG.CONFIRM);

        sendActionPlayAnim(curLesson.conditionTrigger.name);
    }
    private void SwitchDialog(DIALOG dialog)
    {
        if (curDialog != null) {
            ActivateDialog(curDialog, false);
        } 
        switch(dialog)
        {
            case DIALOG.CONFIRM:
                ActivateDialog(confirmDialog, true);
                curDialog = confirmDialog;
                break;
            case DIALOG.PLAY:
                ActivateDialog(playDialog, true);
                curDialog = playDialog;
                break;
        }
    }
    private void ActivateDialog(GameObject dialog, bool active)
    {
        if(dialog)
        {
            dialog.SetActive(active);
        }
    }
    /*
    * Send ActionYes Event to client
    */
    public void sendActionYes()
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = "ActionYes";
        ConnectionManager.instance.SendAction(EventCodes.ActionYes, packages);
    }
    public void sendActionNo()
    {
        LessonManager.instance.ChangeLesson();
        curLesson = LessonManager.instance.GetCurLesson();
        object[] packages = new object[3];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = LessonManager.instance.GetLessonIndex();
        ConnectionManager.instance.SendAction(EventCodes.ActionNo, packages);
    }
    public void sendActionPlayAnim(string trigger = "Amimation")
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = trigger;
        ConnectionManager.instance.SendAction(EventCodes.ActionPlayAnimation, packages);
    }
}
