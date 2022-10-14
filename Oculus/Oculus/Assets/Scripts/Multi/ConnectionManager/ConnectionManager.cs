using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public enum EventCodes : byte
{
    ActionYes,
    ActionNo,
    ActionPlayAnimation
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
            switch(theEvent)
            {
                case EventCodes.ActionNo:
                    OnAcionNo(data);
                    break;
                case EventCodes.ActionYes:
                    OnAcionYes(data);
                    break;
                case EventCodes.ActionPlayAnimation:
                    OnAcionPlayAnimaiton(data);
                    break;
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
    public void SendAction(EventCodes eventCode,object[] packages)
    {
        PhotonNetwork.RaiseEvent(
            (byte)eventCode,
            packages,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
            );
    }
    /*
     * Recive a event YES
     */
    public void OnAcionYes(object[] data)
    {
    }
    public void OnAcionNo(object[] data)
    {

    }
    public void OnAcionPlayAnimaiton(object[] data)
    {

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

