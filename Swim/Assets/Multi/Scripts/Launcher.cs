using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    public GameObject Room;
    const string ROOM_NAME= "SWIM";
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnConnectedToMaster()
    {
        // base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to server");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

    }
    public void CreateRoom()
    {
        RoomOptions room  = new RoomOptions();
        room.MaxPlayers = 5;
        room.IsVisible = true;
        PhotonNetwork.CreateRoom(ROOM_NAME, room);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        Room.SetActive(true);
        Room.transform.Find("RoomTitle").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.Name;
        base.OnJoinedRoom();

    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Created" + returnCode +"- " + message);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Room.SetActive(false);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Room OnLeftRoom");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate" + roomList.Count);
        for(int i = 0; i< roomList.Count;i++)
        {
            Debug.Log("@@RoomInfo" + roomList[i].Name);
        }
        //foreach (RoomInfo r in roomList)
        //{
        //    Debug.Log("RoomInfo" + r.Name);
        //}
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(ROOM_NAME);
    }
}
 