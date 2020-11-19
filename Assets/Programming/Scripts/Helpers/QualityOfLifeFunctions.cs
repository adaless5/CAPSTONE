using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QualityOfLifeFunctions
{
    public static List<T> ArrayToList<T>(T[] array)
    {
        List<T> list = new List<T>();

        foreach(T item in array)
        {
            list.Add(item);
        }

        return list;
    }

    public static T[] ListToArray<T>(List<T> list)
    {
        T[] array = new T[list.Count];

        for(int i = 0; i < list.Count; i++)
        {
            array[i] = list[i];
        }
        
        return array;
    }

    public static bool CloseEnough(Vector3 v1, Vector3 v2)
    {
        if (!Mathf.Approximately(v1.x, v2.x)) return false;
        else if (!Mathf.Approximately(v1.y, v2.y)) return false;
        else if (!Mathf.Approximately(v1.z, v2.z)) return false;
        return true;
    }

    public static bool CloseEnough(Quaternion q1, Quaternion q2)
    {
        if (!Mathf.Approximately(q1.x, q2.x)) return false;
        else if (!Mathf.Approximately(q1.y, q2.y)) return false;
        else if (!Mathf.Approximately(q1.z, q2.z)) return false;
        else if (!Mathf.Approximately(q1.w, q2.w)) return false;
        return true;
    }
}
