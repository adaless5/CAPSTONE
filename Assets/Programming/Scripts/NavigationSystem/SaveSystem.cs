using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

//Wrapper around unity's built in persisten save system. This simplifies the overall system. adds new functionality and
//allows for objects with the same name in different scenes to be saved without overwritting accidently.
public class SaveSystem : MonoBehaviour
{
    public delegate void SaveEventDelegate();
    public static SaveEventDelegate SaveEvent;

    public enum SaveType
    {
        DEFAULT,
        CONNECTOR,
        RESPAWNINFO,
    }

    public static void Save(string gameObjectName, string variableName,string sceneName, int val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        PlayerPrefs.SetInt(id, val);

        if (saveType == SaveType.DEFAULT) DefaultIDRegistry.Add(id);
    }

    public static void Save(string gameObjectName, string variableName, string sceneName, string val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        PlayerPrefs.SetString(id, val);

        if (saveType == SaveType.DEFAULT) DefaultIDRegistry.Add(id);
    }

    public static void Save(string gameObjectName, string variableName, string sceneName, float val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        PlayerPrefs.SetFloat(id, val);

        if (saveType == SaveType.DEFAULT) DefaultIDRegistry.Add(id);
    }

    public static void Save(string gameObjectName, string variableName, string sceneName, bool val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        switch (val)
        {
            case false: PlayerPrefs.SetInt(id, 0); break;
            case true: PlayerPrefs.SetInt(id, 1); break;
        }

        if (saveType == SaveType.DEFAULT) DefaultIDRegistry.Add(id);
    }

    public static void StaticSaveString(string key, string val, SaveType saveType = SaveType.DEFAULT)
    {
        PlayerPrefs.SetString(key, val);
    }

    public static int LoadInt(string gameObjectName, string variableName, string sceneName)
    {
        return PlayerPrefs.GetInt(GetSaveID(gameObjectName, variableName, sceneName));
    }

    public static bool LoadBool(string gameObjectName, string variableName, string sceneName)
    {
        int val = PlayerPrefs.GetInt(GetSaveID(gameObjectName, variableName, sceneName));
        switch (val)
        {
            case 0: return false;
            case 1: return true;
            default: return false; //A little dangerous, but good until i find a workaround.
        }
    }

    public static string LoadString(string gameObjectName, string variableName, string sceneName)
    {
        return PlayerPrefs.GetString(GetSaveID(gameObjectName, variableName, sceneName));
    }

    public static string StaticLoadString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static float LoadFloat(string gameObjectName, string variableName, string sceneName)
    {
        return PlayerPrefs.GetFloat(GetSaveID(gameObjectName, variableName,sceneName));
    }

    public static void RemoveAtKey(string gameObjectName, string variableName, string sceneName)
    {
        PlayerPrefs.DeleteKey(GetSaveID(gameObjectName, variableName, sceneName));
    }

    public static void ResetSaveEventDelegateList()
    {
        //Remove all delegates subscribed to SaveEvent.
        foreach (System.Delegate d in SaveEvent.GetInvocationList())
        {
            SaveEvent -= (SaveEventDelegate)d;
        }
    }

    public static string GetSaveID(string gameObjectName, string variableName, string sceneName)
    {
        return (gameObjectName + variableName + sceneName);
    }

    /// 
    /// RespawnInfo Registry
    ///

    public const string RESPAWN_INFO_REGISTRY_ID = "_respawnInfo";
    public class RespawnInfo_Data
    {
        public Vector3 pos;
        public Quaternion rot;
        public string sceneName { get; set; }

        public RespawnInfo_Data(Transform t, string sceneName)
        {
            pos = t.position;
            rot = t.rotation;
            this.sceneName = sceneName;
        }
        public RespawnInfo_Data()
        {
            pos = Vector3.zero;
            rot = Quaternion.identity;
            sceneName = null;
        }

        public override string ToString()
        {
            return pos.x + "~" + pos.y + "~" + pos.z + "~"
                 + rot.x + "~" + rot.y + "~" + rot.z + "~" + rot.w + "~"
                 + sceneName + "~";
        }

        public void FromString(string str)
        {
            string s = "";
            int i = 0;
            foreach (char c in str)
            {
                if (c.Equals('~'))
                {
                    switch (i)
                    {
                        case 0:
                            pos.x = float.Parse(s); break;
                        case 1:
                            pos.y = float.Parse(s); break;
                        case 2:
                            pos.z = float.Parse(s); break;
                        case 3:
                            rot.x = float.Parse(s); break;
                        case 4:
                            rot.y = float.Parse(s); break;
                        case 5:
                            rot.z = float.Parse(s); break;
                        case 6:
                            rot.w = float.Parse(s); break;
                        case 7:
                            sceneName = s; break;
                    }
                    i++; s = "";
                }
                else s += c;
            }
        }
    }

    public static void SaveRespawnInfo(Transform playerTransform, string sceneName)
    {
        RespawnInfo_Data data = new RespawnInfo_Data(playerTransform, sceneName);
        StaticSaveString(RESPAWN_INFO_REGISTRY_ID, data.ToString(), SaveType.RESPAWNINFO);
    }

    public static RespawnInfo_Data FetchRespawnInfo()
    {
        RespawnInfo_Data data = new RespawnInfo_Data();
        data.FromString(StaticLoadString(RESPAWN_INFO_REGISTRY_ID));
        return data;
    }

    /// 
    /// RespawnInfo Registry END
    ///
}