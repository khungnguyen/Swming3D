using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StudentModelManager : MonoBehaviour, IReceiver
{
    public static StudentModelManager instance;

    private GameObject curStudent;
    private GameObject preStudent;
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
    }
    void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
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
                var point = SpawnPointManager.instance.GetStudentSpawnPointByName(pointName);
                curStudent = SpawnStudent(model, point);
                var controller = curStudent.GetComponent<StudentController>();
                controller.SetAnimator(animator, true);
                controller.TriggerAnimation(animation);
                PhotonNetwork.Destroy(preStudent);
               
            }
            else
            {
                 Utils.Log(this,"EventCodes.ActionChangeModel");
            }

            //Destroy(preStudent);

        }
    }
    public void OnStudentCreate(StudentController st)
    {
        curStudent = st.gameObject;
        Utils.Log(this,"EventCodes.OnStudentCreate");
    }
}
