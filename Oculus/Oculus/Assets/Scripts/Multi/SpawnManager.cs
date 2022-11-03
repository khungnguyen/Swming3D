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
        if(DectectVR.instancne.isVR)
        {
            Transform point = SpawnPointManager.instance.GetXRPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate(convertPrefabPath(XRPlayerGO), point.position, point.rotation);

            point = SpawnPointManager.instance.GetStudentSpawnPoint();
            PhotonNetwork.Instantiate(convertPrefabPath(studentGO), point.transform.position, point.transform.rotation);
        }
        else
        {
            Transform point = SpawnPointManager.instance.GetDesktopPlayerSpawnPoint();
            player = PhotonNetwork.Instantiate(convertPrefabPath(DesktopPlayerGO), point.position, point.rotation);
        }
    
    }
   private string convertPrefabPath(GameObject gameObject)
    {
        return MULTI_PREFAB_PATH + "/" + gameObject.name;
    }
}
