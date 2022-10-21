using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> modelsHolderLessonTwo;

    public static ModelHolder instance;
    void Awake() {
        instance = this;
    }
    public GameObject FindModelByName(string name) {
        return modelsHolderLessonTwo.Find(e=>e.name==name);
    }
}
