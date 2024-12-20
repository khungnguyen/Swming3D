using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StudentModelManager : MonoBehaviourPunCallbacks, IReceiver, IPunInstantiateMagicCallback
{
    public static StudentModelManager instance;

    private GameObject curStudent;
    private GameObject preStudent;

    [SerializeField]
    private ExerciseActionControl examinerUI;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawDefaultStudent();

        }

    }
    public void SpawDefaultStudent()
    {
        if (DectectVR.instancne.isVR || VRAppDebug.USE_DEBUG_NO_VR_SINGLE_PREVIEW)
        {
            var point = SpawnPointManager.instance.GetStudentSpawnPointByName("Lesson_1_Ex_All_Pos");
            curStudent = SpawnStudent("StudentFullter", point);
            curStudent.GetPhotonView().RPC("InitFirstPose", RpcTarget.All);
            //  curStudent.GetComponent<StudentController>().InitFirstPose();
        }
    }
    public GameObject SpawnStudent(string name, Transform point)
    {
        return SpawnManager.instance.SpawnStudent(name, point);
    }
    void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);
        base.OnEnable();
    }
    void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
        base.OnDisable();
    }

    public void OnActionReceiver(EventCodes theEvent, object[] packages)
    {
        if (theEvent == EventCodes.ActionChangeModel)
        {
            if (DectectVR.instancne.isVR || VRAppDebug.USE_DEBUG_NO_VR_SINGLE_PREVIEW)
            {
                preStudent = curStudent;
                curStudent.SetActive(false);
                string model = (string)packages[0];
                string pointName = (string)packages[1];
                string animator = (string)packages[2];
                string animation = (string)packages[3];
                string interact = (string)packages[4];
                string moving = (string)packages[5];
                var point = SpawnPointManager.instance.GetStudentSpawnPointByName(pointName);
                Utils.Log(this, "ActionChangeModel", model, pointName, animator, animation, interact,moving);
                curStudent = SpawnStudent(model, point);
                var photonView = curStudent.GetPhotonView();

                photonView.RPC("SetAnimator", RpcTarget.All, animator, true);
                if (interact.Equals("DelayAfterAnim"))
                {
                    photonView.RPC("EnableInteractionDelay", RpcTarget.All, true);
                }
                else if (interact.Equals("True"))
                {
                    photonView.RPC("EnableInteractionImmediate", RpcTarget.All, true);
                }
                if(!animation.Equals(""))
                {
                    photonView.RPC("TriggerAnimation", RpcTarget.All, animation);
                } 
                photonView.RPC("CorrectTransform", RpcTarget.All, pointName);
                if(moving.Equals("True"))
                {
                    photonView.RPC("ActivateBodyMoving", RpcTarget.All, true);
                }
                PhotonNetwork.Destroy(preStudent);

            }
            else
            {
                Utils.Log(this, "EventCodes.ActionChangeModel");
            }

            //Destroy(preStudent);

        }
    }
    public void OnStudentCreate(StudentController st)
    {
        curStudent = st.gameObject;
        Utils.Log(this, "EventCodes.OnStudentCreate");
        st.OnAnimationStateComplete += OnAnimationStateComplete;
    }

    public void OnAnimationStateComplete(bool b)
    {
        if (examinerUI)
        {
            examinerUI.EnableButtons(true);
        }

    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // curStudent = info.Sender.TagObject;
        Utils.LogError(this, "OnPhotonInstantiate TagObject", info.Sender.TagObject);
        Utils.LogError(this, "OnPhotonInstantiate", info.photonView.transform.name);
        Utils.LogError(this, "OnPhotonInstantiate", info.photonView.gameObject.GetComponent<StudentController>());
    }
}
