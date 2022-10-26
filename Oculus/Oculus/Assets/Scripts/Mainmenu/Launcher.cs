using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum MENU
{
    RoomListMenu,
    RoomMenu,
    Loading,
    MainMenu,
    All,
    None
}
public class Launcher : MonoBehaviourPunCallbacks, RoomButtonCallback
{
    public static Launcher instance;
    [SerializeField]
    private GameObject Mainmenu;
    [SerializeField]
    private GameObject RoomListMenu;

    [SerializeField]
    private GameObject RoomMenu;

    [SerializeField]
    private GameObject Loading;
    const string ROOM_NAME = "SWIM";

    [SerializeField]
    GameObject RoomItemPrefab;

    [SerializeField]
    GameObject PlayerInfoPrefab;

    List<RoomItem> roomItemListCached = new List<RoomItem>();
    List<PlayerInfo> playerInfoListCached = new List<PlayerInfo>();

    private MENU curMenu = MENU.None;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Guest" + Random.Range(1, 100);
        ActivateMenu(MENU.All, false, MENU.MainMenu);
        ActivateMenu(MENU.Loading, true);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnConnectedToMaster()
    {
        // base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Connected to server");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        ActivateMenu(MENU.Loading, false);

    }
    public void CreateRoom()
    {
        ActivateMenu(MENU.Loading, true);
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 2;
        room.IsVisible = true;
        PhotonNetwork.CreateRoom(ROOM_NAME + Random.Range(0, 1000), room);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        ActivateMenu(MENU.Loading, false);
        ActivateMenu(MENU.RoomMenu, true);
        RoomMenu.transform.Find("RoomTitle").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.Name;
        ListAllPlayers();
    }
    public override void OnCreatedRoom()
    {
        ActivateMenu(MENU.Loading, false);
        Debug.Log("Room Created");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Created" + returnCode + "- " + message);
        ActivateMenu(MENU.Loading, false);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        ActivateMenu(MENU.Loading, false);
        Debug.Log("Can't connect" + cause.ToString());
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ActivateMenu(MENU.RoomMenu, false, MENU.MainMenu);
    }
    public void CloseRoomListMenu()
    {
        ActivateMenu(MENU.RoomListMenu, false, MENU.MainMenu);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Room OnLeftRoom");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate" + roomList.Count);
        foreach (RoomItem room in roomItemListCached)
        {
            Destroy(room.gameObject);
        }
        roomItemListCached.Clear();
        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log("@@RoomInfo" + roomList[i].Name);
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                GameObject roomObject = Instantiate(RoomItemPrefab);
                roomObject.transform.SetParent(RoomListMenu.transform.Find("Scroll View/Viewport/Content"));
                roomObject.transform.localScale = new Vector3(1, 1, 1);
                RoomItem item = roomObject.GetComponent<RoomItem>();
                item.setRoomInfo(roomList[i]);
                item.setListener(this);
                roomItemListCached.Add(item);
            }

        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom" + newPlayer.NickName);
        Debug.Log("PhotonNetwork.PlayerList" + PhotonNetwork.PlayerList.Length);
        ListAllPlayers();
    }
    private void ListAllPlayers()
    {
        foreach (PlayerInfo p in playerInfoListCached)
        {
            Destroy(p.gameObject);
        }
        playerInfoListCached.Clear();
        foreach (Player newPlayer in PhotonNetwork.PlayerList)
        {
            Debug.Log("reach" + newPlayer.NickName);
            GameObject playerObject = Instantiate(PlayerInfoPrefab);
            playerObject.transform.SetParent(RoomMenu.transform.Find("Scroll View/Viewport/Content"));
            playerObject.transform.localScale = new Vector3(1, 1, 1);
            PlayerInfo comp = playerObject.GetComponent<PlayerInfo>();
            comp.setPlayerInfo(newPlayer);
            playerInfoListCached.Add(comp);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom" + otherPlayer);
        ListAllPlayers();
    }
    public void JoinRoom(string room = ROOM_NAME)
    {
        PhotonNetwork.JoinRoom(room);
    }
    public void StartGame()
    {
        if (!DectectVR.instancne.isVR)
        {
            ScenesManager.instance.GoTo(SCREEN.Main, true);
        }
    }
    public void FindRoom()
    {
        ActivateMenu(MENU.RoomListMenu, true);
    }
    public void OnRoomButtonClick(string roomName)
    {
        Debug.Log("OnRoomButtonClick" + roomName);
        JoinRoom(roomName);
    }
    public void ActivateMenu(MENU menu, bool active, MENU switchTo = MENU.None)
    {
        switch (menu)
        {
            case MENU.RoomListMenu:
                RoomListMenu.SetActive(active);
                break;
            case MENU.MainMenu:
                Mainmenu.SetActive(active);
                break;
            case MENU.RoomMenu:
                RoomMenu.SetActive(active);
                break;
            case MENU.Loading:
                Loading.SetActive(active);
                break;
            case MENU.None:
                break;
            default:
                RoomListMenu.SetActive(active);
                RoomMenu.SetActive(active);
                Loading.SetActive(active);
                Mainmenu.SetActive(active);
                break;
        }
        if (switchTo != MENU.None && !active)
        {
            ActivateMenu(switchTo, true, MENU.None);
        }

    }
    public void ExitApplication()
    {
        Application.Quit();
    }
}
