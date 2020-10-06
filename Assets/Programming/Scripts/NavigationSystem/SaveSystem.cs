using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveSystem : MonoBehaviour
{
    public enum LoadType
    {
        INT,
        STRING,
        FLOAT,
        OBJECT,
        BOOL,
    }

    bool isFirstLoad = true;

    private static string _sceneName;

    public delegate void SaveEventDelegate();

    public static SaveEventDelegate SaveEvent;

    public static void Save(string gameObjectName, string variableName, int val)
    {
        PlayerPrefs.SetInt(GetSaveID(gameObjectName,variableName), val);
    }

    public static void Save(string gameObjectName, string variableName, string val)
    {
        PlayerPrefs.SetString(GetSaveID(gameObjectName,variableName), val);
    }

    public static void Save(string gameObjectName, string variableName, float val)
    {
        PlayerPrefs.SetFloat(GetSaveID(gameObjectName,variableName), val);
    }

    public static void Save(string gameObjectName, string variableName, bool val)
    {
        switch (val)
        {
            case false: PlayerPrefs.SetInt(GetSaveID(gameObjectName,variableName), 0); break;
            case true: PlayerPrefs.SetInt(GetSaveID(gameObjectName,variableName), 1); break;
        }
    }

    public static void Save<T>(string gameObjectName, string variableName, T obj)
    {
        string json = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(GetSaveID(gameObjectName,variableName), json);
    }

    public static int LoadInt(string gameObjectName, string variableName)
    {
        return PlayerPrefs.GetInt(GetSaveID(gameObjectName,variableName));
    }

    public static bool LoadBool(string gameObjectName, string variableName)
    {
        int val = PlayerPrefs.GetInt(GetSaveID(gameObjectName,variableName));
        switch (val)
        {
            case 0: return false;
            case 1: return true;
            default: return false; //A little dangerous, but good until i find a workaround.
        }
    }

    public static string LoadString(string gameObjectName, string variableName)
    {
        return PlayerPrefs.GetString(GetSaveID(gameObjectName,variableName));
    }

    public static float LoadFloat(string gameObjectName, string variableName)
    {
        return PlayerPrefs.GetFloat(GetSaveID(gameObjectName,variableName));
    }

    public static T LoadObject<T>(string gameObjectName, string variableName)
    {

        T obj = JsonUtility.FromJson<T>(GetSaveID(gameObjectName,variableName));
        return obj;

    }

    public static void ResetSaveEventDelegateList()
    {
        
        //Remove all delegates subscribed to SaveEvent.
        foreach (System.Delegate d in SaveEvent.GetInvocationList())
        {
            SaveEvent -= (SaveEventDelegate)d;
        }

    }

    public static string GetSaveID(string gameObjectName, string variableName)
    {
        return (gameObjectName + variableName + SceneManager.GetActiveScene().name);
    }



}
