
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static bool IsPointerOverUI(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        foreach (RaycastResult raysastResult in raysastResults)
        {
            if (raysastResult.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
    public static void Logging(string tag, LogLevel level, params object[] agr)
    {

        string message = "";
        if (agr != null) { }
        for (int i = 0; i < agr.Length; i++)
        {
            if (agr[i] != null)
            {
                message += agr[i].ToString();

            }
            else
            {
                message += "null";
            }
            message += " ";
        }
        if (level == LogLevel.Debug)
        {
            Debug.Log("#[" + tag + "] " + message);
        }
        else if (level == LogLevel.Error)
        {
            Debug.LogError("#[" + tag + "] " + message);
        }

    }
    public static void Log(object tag, params object[] agr)
    {
        if (tag is string)
        {

            Logging((string)tag, LogLevel.Debug, agr);

        }
        else
        {
            Logging(tag != null ? tag.GetType().Name : "No Name", LogLevel.Debug, agr);
        }

    }
    public static void LogError(object tag, params object[] agr)
    {
        if (tag is string)
        {
            Logging((string)tag, LogLevel.Error, agr);

        }
        else
        {
            Logging(tag != null ? tag.GetType().Name : "No Name", LogLevel.Error, agr);
        }

    }
    public static T String2Enum<T>(string t)
    {
        return (T)System.Enum.Parse(typeof(T), t);
    }
    public static void DestroyTransformChildren(Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Transform.Destroy(transform.GetChild(i).gameObject);
        }
    }
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
    public static T CopyComponentV2<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }
    public static Transform FindChild(Transform parent, string name, bool removeSpace = false)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var childName = child.name;
            if (removeSpace)
            {
                childName = childName.Replace(" ", "");
            }
            if (childName.Contains(name))
                return child;
            var result = FindChild(child, name, removeSpace);
            if (result != null)
                return result;
        }
        return null;
    }
    public static Transform FindChildRecursive(this Transform parent, string name, bool removeSpace)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var childName = child.name;
            if (removeSpace)
            {
                childName = childName.ReplaceAll(" ", "");
            }
            if (childName.Contains(name))
            {

                return child;
            }


            var result = child.FindChildRecursive(name, removeSpace);
            if (result != null)
                return result;
        }
        return null;
    }
    public static string ReplaceAll(this string input, string find, string replace)
    {
        while (input.Contains(find))
        {
            input = input.Replace(find, replace);
        }
        return input;
    }
    public static IList GetList(Type type)
    {
        string t = type.ToString();
        string sub = t[(t.IndexOf("[") + 1)..t.IndexOf("]")].Trim();
        Log("sub", sub);
        var newType = FindObjectsOfTypeByName(sub);
        Log("GetList", newType);
        return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(newType));
    }
    public static Type FindObjectsOfTypeByName(string aClassName)
    {
        Type textType = null;
        string typeName = aClassName;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            textType = assembly.GetType(typeName);
            if (textType != null)
                break;
        }
        return textType;
    }
}
