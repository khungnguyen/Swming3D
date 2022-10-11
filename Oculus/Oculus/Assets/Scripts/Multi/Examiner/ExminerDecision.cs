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
    void Start()
    {
        ActivateDialog(playDialog, false);
        curDialog = confirmDialog;
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
        sendActionPlayAnim("Quy Send Animion");
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
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = "ActionNo";
        ConnectionManager.instance.SendAction(EventCodes.ActionNo, packages);
    }
    public void sendActionPlayAnim(string anim = "Amimation")
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = anim;
        ConnectionManager.instance.SendAction(EventCodes.ActionPlayAnimation, packages);
    }
}
