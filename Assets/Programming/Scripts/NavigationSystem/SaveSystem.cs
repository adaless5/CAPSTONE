using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

//Wrapper around unity's built in persisten save system. This simplifies the overall system. adds new functionality and
//allows for objects with the same name in different scenes to be saved without overwritting accidently.
public class SaveSystem : MonoBehaviour
{
    static bool bDebug = true;

    public enum SaveType
    {
        DEFAULT,
        CONNECTOR,
        RESPAWNINFO,
        EQUIPMENT,
    }

    public static void Save(string gameObjectName, string variableName,string sceneName, int val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        PlayerPrefs.SetInt(id, val);

        FileIO.SaveToFile(sceneName, gameObjectName + "_" + variableName, val);

        if (saveType == SaveType.DEFAULT || saveType == SaveType.EQUIPMENT) DefaultIDRegistry.Add(id);
    }

    public static void Save(string gameObjectName, string variableName, string sceneName, string val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        PlayerPrefs.SetString(id, val);

        FileIO.SaveToFile(sceneName, gameObjectName + "_" + variableName, val);


        if (saveType == SaveType.DEFAULT || saveType == SaveType.EQUIPMENT) DefaultIDRegistry.Add(id);

        if (bDebug) Debug.Log(val);
    }

    public static void Save(string gameObjectName, string variableName, string sceneName, float val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        PlayerPrefs.SetFloat(id, val);

        FileIO.SaveToFile(sceneName, gameObjectName + "_" + variableName, val);

        if (saveType == SaveType.DEFAULT || saveType == SaveType.EQUIPMENT) DefaultIDRegistry.Add(id);

        if (bDebug) Debug.Log(val);
    }

    public static void Save(string gameObjectName, string variableName, string sceneName, bool val, SaveType saveType = SaveType.DEFAULT)
    {
        string id = GetSaveID(gameObjectName, variableName, sceneName);
        switch (val)
        {
            case false: PlayerPrefs.SetInt(id, 0); break;
            case true: PlayerPrefs.SetInt(id, 1); break;
        }
        FileIO.SaveToFile(sceneName, gameObjectName + "_" + variableName, val);

        if (saveType == SaveType.DEFAULT || saveType == SaveType.EQUIPMENT) DefaultIDRegistry.Add(id);
    }

    public static void StaticSaveString(string key, string val, SaveType saveType = SaveType.DEFAULT)
    {
        PlayerPrefs.SetString(key, val);
    }

    public static void SaveConnector(string connectorName, string sceneName, string data)
    {
        Debug.Log("TEST"+data);
        FileIO.SaveConnectorToFile(sceneName, connectorName, data);
    }

    public static int LoadInt(string gameObjectName, string variableName, string sceneName)
    {
        //return PlayerPrefs.GetInt(GetSaveID(gameObjectName, variableName, sceneName));
        string s = FileIO.LoadFromFile(sceneName, gameObjectName + "_" + variableName);
        if (s == null) return 0;

        return int.Parse(s);
    }

    public static bool LoadBool(string gameObjectName, string variableName, string sceneName)
    {
        //int val = PlayerPrefs.GetInt(GetSaveID(gameObjectName, variableName, sceneName));
        //switch (val)
        //{
        //    case 0: return false;
        //    case 1: return true;
        //    default: return false; //A little dangerous, but good until i find a workaround.
        //}
        string s = FileIO.LoadFromFile(sceneName, gameObjectName + "_" + variableName);
        if (s == null) return false;

        return bool.Parse(s);
    }

    public static string LoadString(string gameObjectName, string variableName, string sceneName)
    {
        //return PlayerPrefs.GetString(GetSaveID(gameObjectName, variableName, sceneName));
        string s = FileIO.LoadFromFile(sceneName, gameObjectName + "_" + variableName);
        if (s == null) return "";

        return s;
    }

    public static string LoadConnector(string connectorName, string sceneName)
    {
        string s = FileIO.LoadConnectorFromFile(sceneName, connectorName);
        if (s == null) return "";

        return FileIO.LoadConnectorFromFile(sceneName, connectorName);
    }

    public static string StaticLoadString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static float LoadFloat(string gameObjectName, string variableName, string sceneName)
    {
        string s = FileIO.LoadFromFile(sceneName, gameObjectName + "_" + variableName);
        if (s == null) return 0;

        return float.Parse(s);
    }

    public static void RemoveAtKey(string gameObjectName, string variableName, string sceneName)
    {
        PlayerPrefs.DeleteKey(GetSaveID(gameObjectName, variableName, sceneName));
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