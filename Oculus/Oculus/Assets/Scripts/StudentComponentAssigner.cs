using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StudentComponentAssigner : MonoBehaviour
{
    public string defaultHumanoid = "Bip001";
    public GameObject sourceGameObject;

    private Transform _humanoidBody;
    public Transform humanoidBody
    {
        get
        {
            if (_humanoidBody == null)
            {
                return _humanoidBody = transform.Find(defaultHumanoid);
            }
            else {
                return _humanoidBody;
            }

        }
    }
}
