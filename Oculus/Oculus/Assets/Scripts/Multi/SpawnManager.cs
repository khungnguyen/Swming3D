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

    public void SpawnPlayers()
    {
        if(DectectVR.instancne.isVR)
        {
            Transform transfrom = SpawnPointManager.instance.GetXRPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate(convertPrefabPath(XRPlayerGO), transfrom.position, transform.rotation);

            Transform point = SpawnPointManager.instance.GetStudentSpawnPoint();
            var gameObject = PhotonNetwork.Instantiate(convertPrefabPath(studentGO), point.transform.position, point.transform.rotation);
                //gameObject.transform.SetParent(point);
        }
        else
        {
            Transform transfrom = SpawnPointManager.instance.GetDesktopPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate(convertPrefabPath(DesktopPlayerGO), transfrom.position, transform.rotation);
        }
  
    }
   private string convertPrefabPath(GameObject gameObject)
    {
        return MULTI_PREFAB_PATH + "/" + gameObject.name;
    }
}
