using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public interface RoomButtonCallback {
    void OnRoomButtonClick(string m);
}
public class RoomItem : MonoBehaviour
{
    private RoomInfo info;
    [SerializeField]
    private TMP_Text title;

    private RoomButtonCallback callback;
    public void setListener(RoomButtonCallback callback) {
        this.callback = callback;
    }
    public void setRoomInfo(RoomInfo info) {
        this.info = info;
        title.text = info.Name;
    }
    public void RoomClick() {
        if(callback!=null) {
            callback.OnRoomButtonClick(info.Name);
        }
    }
}
