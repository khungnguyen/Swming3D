using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private List<Dialog> dialogPrefabs;

    [SerializeField]
    private List<GameObject> studentModels;

    [SerializeField]
    private GameObject examinerGameObject;

     [SerializeField]
    private GameObject coachGameObject;
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
    public GameObject GetStudentModelByName(string name) {
        return studentModels.Find(e=>e.name==name);
    }
    public GameObject GetExaminerGameObject() {
        return examinerGameObject;
    }
    public GameObject GetCoachGameObject() {
        return coachGameObject;
    }
}
