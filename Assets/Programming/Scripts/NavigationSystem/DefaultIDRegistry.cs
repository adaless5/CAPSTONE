using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class DefaultIDRegistry 
{
    static bool bDebug = false;

    public const string DEFAULT_ID_REGISTRY_KEY = "_defaultIDs";
    static List<string> _DefaultIDs;

    public static List<string> GetRegistry()
    {
        //Query persistent data for registry
        string registryStr = SaveSystem.StaticLoadString(DEFAULT_ID_REGISTRY_KEY);

        //if registry doesn't exist in persistent memory return null.
        if (registryStr == "") return null;

        //else return the registry.
        return FromString(registryStr);
    }

    public static void Add(string obj)
    {
        //Get registry from persistant data.
        _DefaultIDs = GetRegistry();

        //Create registry if null;
        if (_DefaultIDs == null) _DefaultIDs = new List<string>();

        //Add item to registry
        _DefaultIDs.Add(obj);
        if (bDebug) Debug.Log("Scene Connector Registry [Added to Registry] : " + obj.ToString());

        //Update saved registry in persistent data persistent data.
        SaveSystem.StaticSaveString(DEFAULT_ID_REGISTRY_KEY, RegistryToString(_DefaultIDs));
    }

    public static void DeleteAll()
    {
        //Get registry from persistant data.
        _DefaultIDs = GetRegistry();

        //If it doesnt exist return.
        if (_DefaultIDs == null) { if (bDebug) Debug.Log("DEFAULT ID Registry : No Saved Data Found"); return; }

        //Loop Through all id's and remove them from player prefs.
        foreach (string id in _DefaultIDs)
        {
            PlayerPrefs.DeleteKey(id);
        }

        Debug.Log("DEFAULT ID Registry : " + _DefaultIDs.Count + " Saved Entries Successfully Removed");

        //Delete the registry from player prefs.
        PlayerPrefs.DeleteKey(DEFAULT_ID_REGISTRY_KEY);
    }

    public static string RegistryToString(List<string> defaultIDs)
    {
        string str = "";

        foreach (string item in defaultIDs)
        {
            str += item.ToString() + "|";
        }
        return str;
    }

    public static List<string> FromString(string str)
    {
        string s = "";
        _DefaultIDs = new List<string>();
        foreach (char c in str)
        {
            if (c.Equals('|'))
            {
                _DefaultIDs.Add(s);
                s = "";
            }
            else s += c;
        }
        return _DefaultIDs;
    }
}
