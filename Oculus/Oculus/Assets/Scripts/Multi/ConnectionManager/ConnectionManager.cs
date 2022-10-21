using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public enum EventCodes : byte
{
    ActionStartExercise,
    ActionNextExercise,
    ActionPlayAnimation,
    ActionEnableInteractable,
    ActionDisableInteractable,
    ActionSwimDistance,
    ActionInitLesson,
    ActionInitStartPoint,
    ActionSettingUpLesson,
    ActionReplaceModel,

    ActionResetModel
}
public class ConnectionManager : MonoBehaviourPunCallbacks,IOnEventCallback
{
    public static ConnectionManager instance;
    private static List<IReciever> receivers = new List<IReciever>();
   
    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Photon Doesn't connect to server");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
       if(photonEvent.Code < 200)
        {
            EventCodes theEvent = (EventCodes)photonEvent.Code;
            object[] data = (object[])photonEvent.CustomData;
            foreach (var re in receivers)
            {
                re.OnActionReciver(theEvent, data);
            }
        }
    }
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /*
     * Master will send event to Client
     * eve : EventCodes
     * data : should be Animation Info
     */
    public void SendAction(EventCodes eventCode,object[] packages,ReceiverGroup group = ReceiverGroup.All)
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.RaiseEvent(
                       (byte)eventCode,
                       packages,
                       new RaiseEventOptions { Receivers = group },
                       new SendOptions { Reliability = true }
                       );
        }
        else
        {
            Debug.Log("No Connection Yet");
        }
       
    }
    public static void AddCallbackTarget(IReciever re)
    {
        IReciever  reuslt = receivers.Find(e => e == re);
        if(reuslt == null)
        {
            receivers.Add(re);
            Debug.LogError("Adding callback" + re);
        }
    }
    public static void RemoveCallBackTarget(IReciever re)
    {
        int index = receivers.FindIndex(e => e == re);
        if(index != -1)
        {
            receivers.RemoveAt(index);
        }
    }
}

