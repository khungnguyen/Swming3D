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
    ActionResetLesson,
    ActionInitStartPoint,
    ActionSettingUpLesson,
    ActionReplaceModel,

    ActionChangeController,
    ActionCorrectTransform,

    ActionResetModel,
    ActionStopAnimation,

    ActionActivateExtension,
    ActionBodyMoving,
    ActionChangeModel,
    ActionRotate,
    
}
public class ConnectionManager : MonoBehaviourPunCallbacks,IOnEventCallback
{
    public static ConnectionManager instance;
    private static List<IReceiver> receivers = new List<IReceiver>();
    
    const string TAG="[ConnectionManager]";
    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            Debug.LogError(TAG +"Photon Doesn't connect to server");
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
            foreach (var re in receivers.ToArray())
            {
                re?.OnActionReceiver(theEvent, data);
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
            Debug.Log(TAG+"No Connection Yet");
        }
       
    }
    public static void AddCallbackTarget(IReceiver re)
    {
        IReceiver  result = receivers.Find(e => e == re);
        if(result == null)
        {
            receivers.Add(re);
            // Debug.LogError(TAG+"Adding callback" + re);
        }
    }
    public static void RemoveCallBackTarget(IReceiver re)
    {
        int index = receivers.FindIndex(e => e == re);
        if(index != -1)
        {
            receivers.RemoveAt(index);
            // Debug.LogError(TAG+" Removing callback" + re);
        }
    }
}

