using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private List<Dialog> dialogPrefabs;
    public static ResourceManager instance;
    void Awake()
    {
        instance = this;
    }
    public T CreateDialog<T>(DialogType type,Transform parent) {
        var go = dialogPrefabs.Find(e=>e.name == type.ToString()).gameObject;
        var clone = Instantiate(go,parent);
        return clone.GetComponent<T>();
    }
}
