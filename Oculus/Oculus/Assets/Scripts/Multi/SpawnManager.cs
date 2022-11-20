using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject studentGO;
    [SerializeField]
    private GameObject XRPlayerGO;
    [SerializeField]
    private GameObject DesktopPlayerGO;
    public static SpawnManager instance;

    private GameObject player;

    const string MULTI_PREFAB_PATH = "MultiPrefabs";
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayers();

        }
    }
    /**
    * Spawning player by using photon
    */
    public void SpawnPlayers()
    {
        var resource = ResourceManager.instance;
        if (DectectVR.instancne.isVR)
        {
            Transform point = SpawnPointManager.instance.GetXRPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate(ConvertPrefabPath(resource.GetCoachGameObject()), point.position, point.rotation);
        }
        else
        {
            Transform point = SpawnPointManager.instance.GetDesktopPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate(ConvertPrefabPath(resource.GetExaminerGameObject()), point.position, point.rotation);
        }

    }
    public GameObject SpawnStudent(string model, Transform refer)
    {
        var resource = ResourceManager.instance;
        return PhotonNetwork.Instantiate(ConvertPrefabPath(resource.GetStudentModelByName(model)), refer.transform.position, refer.transform.rotation);

    }
    private string ConvertPrefabPath(GameObject gameObject)
    {
        return MULTI_PREFAB_PATH + "/" + gameObject.name;
    }
}
