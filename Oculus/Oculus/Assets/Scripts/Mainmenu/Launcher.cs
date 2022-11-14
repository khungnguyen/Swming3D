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
public class Launcher : MonoBehaviourPunCallbacks, RoomButtonCallback, IButtonEvent
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

    [SerializeField]
    private GameObject dialogRooms;

    public bool useNewUI = false;
    List<RoomItem> roomItemListCached = new List<RoomItem>();
    List<PlayerInfo> playerInfoListCached = new List<PlayerInfo>();

    private List<RoomInfo> curRooms;

    private DialogScroll selectedRoom;

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
        if (useNewUI)
        {
            ShowSelectedRoom(PhotonNetwork.CurrentRoom.Name);
        }
        else
        {
            ActivateMenu(MENU.Loading, false);
            ActivateMenu(MENU.RoomMenu, true);
            RoomMenu.transform.Find("RoomTitle").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.Name;
            ListAllPlayers();
        }
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
    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        Debug.Log("OnRoomListUpdate" + rooms.Count);
        foreach (RoomItem room in roomItemListCached)
        {
            Destroy(room.gameObject);
        }
        roomItemListCached.Clear();
        for (int i = 0; i < rooms.Count; i++)
        {
            Debug.Log("@@RoomInfo" + rooms[i].Name);
            if (rooms[i].PlayerCount != rooms[i].MaxPlayers && !rooms[i].RemovedFromList)
            {
                GameObject roomObject = Instantiate(RoomItemPrefab);
                roomObject.transform.SetParent(RoomListMenu.transform.Find("Scroll View/Viewport/Content"));
                roomObject.transform.localScale = new Vector3(1, 1, 1);
                RoomItem item = roomObject.GetComponent<RoomItem>();
                item.setRoomInfo(rooms[i]);
                item.setListener(this);
                roomItemListCached.Add(item);
            }

        }
        curRooms = rooms;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom" + newPlayer.NickName);
        Debug.Log("PhotonNetwork.PlayerList" + PhotonNetwork.PlayerList.Length);
        if (useNewUI)
        {
            AddPlayerToRoom(newPlayer);
        }
        else
        {
            ListAllPlayers();
        }
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
        if (useNewUI)
        {
            RemovePlayerFromRoom(otherPlayer);
        }
        else
        {
            ListAllPlayers();
        }
    }
    public void JoinRoom(string room = ROOM_NAME)
    {
        PhotonNetwork.JoinRoom(room);
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
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
                if (useNewUI)
                {
                    ShowAllRooms();
                }
                else
                {
                    RoomListMenu.SetActive(active);
                }
                break;
            case MENU.MainMenu:
                Mainmenu.SetActive(active);
                break;
            case MENU.RoomMenu:
                if (selectedRoom)
                {

                }
                else
                {
                    RoomMenu.SetActive(active);
                }
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
    private void ShowAllRooms()
    {
        GameObject ob = Instantiate(dialogRooms, transform);
        selectedRoom = ob.GetComponent<DialogScroll>();
        curRooms.ForEach(e =>
        {
            if (e.PlayerCount != e.MaxPlayers && !e.RemovedFromList)
            {
                ButtonBaseRoom butt = (ButtonBaseRoom)selectedRoom.AddButton();
                butt.SetText(e.Name);
                butt.SetDescription("Total Players :" + e.PlayerCount);
                butt.SetData(e);
            }
        });
        selectedRoom.Init("Rooms", (object data) =>
        {
            Utils.Log(this, "Room clicked", ((RoomInfo)data).Name);
            string roomName = ((RoomInfo)data).Name;
            //ShowSelectedRoom(roomName);
            JoinRoom(roomName);
            selectedRoom.Hide();
        }, (object data) =>
        {

        }).Show();


    }
    private void ShowSelectedRoom(string roomName)
    {
        if (selectedRoom == null)
        {
            GameObject ob = Instantiate(dialogRooms, transform);
            selectedRoom = ob.GetComponent<DialogScroll>();
            foreach (Player newPlayer in PhotonNetwork.PlayerList)
            {
                AddPlayerToRoom(newPlayer);
            }
            selectedRoom.Init(roomName, (object data) =>
            {

            }, (object data) =>
            {
                selectedRoom.ClearAllButtons();
                selectedRoom = null;
                LeaveRoom();
            }).Show();
        }

    }

    private void AddPlayerToRoom(Player newPlayer)
    {
        if (selectedRoom != null)
        {
            ButtonBaseRoom butt = (ButtonBaseRoom)selectedRoom.AddButton();
            butt.SetText(newPlayer.NickName);
            butt.SetDescription(newPlayer.IsMasterClient ? "Hosted" : "Guest");
            butt.SetData(newPlayer);
        }
    }
    private void RemovePlayerFromRoom(Player newPlayer) {
         if (selectedRoom != null)
        {
           selectedRoom.RemoveButtonByData(newPlayer);
        }
    }
    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OnClicked(object action)
    {
        throw new System.NotImplementedException();
    }
}
