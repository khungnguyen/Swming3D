using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] SpawnTransforms;
    public static SpawnManager instance;
    private void Awake() {
        instance = this;
    }
    public Transform GetSpawnTranform() {
        return SpawnTransforms[Random.Range(0,SpawnTransforms.Length -1)];
    }
}
