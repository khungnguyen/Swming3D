using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] StutdemtSpawnPoints;
    [SerializeField]
    private Transform[] PlayerSpawnPoints;

    public static SpawnPointManager instance;
    private void Awake() {
        instance = this;
    }
    public Transform GetStudentSpawnPoint() {
        return StutdemtSpawnPoints[Random.Range(0, StutdemtSpawnPoints.Length -1)];
    }
    public Transform GetPlayerSpawnPoint()
    {
        return PlayerSpawnPoints[Random.Range(0, PlayerSpawnPoints.Length - 1)];
    }
}
