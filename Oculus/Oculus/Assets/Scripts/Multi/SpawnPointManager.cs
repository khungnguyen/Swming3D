using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] StutdemtSpawnPoints;
    [SerializeField]
    private Transform[] XRPlayerSpawnPoints;
    [SerializeField]
    private Transform[] DesktopPlayerSpawnPoints;

    public static SpawnPointManager instance;
    private void Awake() {
        instance = this;
    }
    public Transform GetStudentSpawnPoint() {
        return StutdemtSpawnPoints[Random.Range(0, StutdemtSpawnPoints.Length -1)];
    }
    public Transform GetXRPlayerSpawnPoint()
    {
        return XRPlayerSpawnPoints[Random.Range(0, XRPlayerSpawnPoints.Length - 1)];
    }
    public Transform GetDesktopPlayerSpawnPoint()
    {
        return DesktopPlayerSpawnPoints[Random.Range(0, DesktopPlayerSpawnPoints.Length - 1)];
    }
    public Transform GetStudentSpawnPointByName(string name) {
        return (new List<Transform>(StutdemtSpawnPoints)).Find(e=>e.name==name);
    }
}
