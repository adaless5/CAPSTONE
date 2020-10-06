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
}
