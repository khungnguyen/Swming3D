
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils
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
        for (int i = 0; i < agr.Length; i++)
        {
            message += agr[i].ToString();
            message += " ";
        }
        if (level == LogLevel.Debug)
        {
            Debug.Log("#[" + tag+ "] " + message);
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
    public static T String2Enum<T>(string t) {
         return  (T)System.Enum.Parse( typeof(T), t );
    }
}
