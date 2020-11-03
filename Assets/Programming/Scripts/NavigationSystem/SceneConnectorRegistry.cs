using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class SceneConnectorRegistry
{
    static bool bDebug = false;

    static List<SceneConnector.SceneConnectorData> _SCRegistry;
    const string _SCRegistryKey = "_SCRegistry";

    public static List<SceneConnector.SceneConnectorData> GetRegistry()
    {
        //Query persistent data for registry
        string registryStr = SaveSystem.StaticLoadString(_SCRegistryKey);

        //if registry doesn't exist in persistent memory return null.
        if (registryStr == "") return null;

        //else return the registry.
        return FromString(registryStr);
    }

    public static void Add(SceneConnector.SceneConnectorData obj)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //Create registry if null;
        if (_SCRegistry == null) _SCRegistry = new List<SceneConnector.SceneConnectorData>();

        //Add item to registry
        _SCRegistry.Add(obj);
        if (bDebug) Debug.Log("Scene Connector Registry [Added to Registry] : " + obj.ToString());

        //Update saved registry in persistent data persistent data.
        SaveSystem.StaticSaveString(_SCRegistryKey, RegistryToString(_SCRegistry));
    }

    //Remove The connector at the given ID from the connector registry
    public static void Remove(string id)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return, theres nothing to remove.
        if (_SCRegistry == null) return;

        //Remove obj from Registry
        foreach(SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.ID.Equals(id)) 
            {
                _SCRegistry.Remove(item);

                if (bDebug) Debug.Log("Scene Connector Registry : Conector removed from registry");
                break;
            }
            if (bDebug) Debug.Log("Scene Connector Registry : Connector was not found");
        }

        //Update saved registry in persistent data persistent data.
        SaveSystem.StaticSaveString(_SCRegistryKey, RegistryToString(_SCRegistry));
    }

    public static bool Contains(string id)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return false.
        if (_SCRegistry == null) return false;

        //Check if registry contains the ID, return true if found. if not return false;
        foreach (SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.ID.Equals(id)) return true;
        }
        return false;
    }

    public static int Size()
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return false.
        if (_SCRegistry == null) return -1;

        return _SCRegistry.Count; 
    }

    public static bool IsEmpty()
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        if (_SCRegistry == null) return true;
        return false; 
    }

    public static SceneConnector.SceneConnectorData GetDataFromID(string id)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return null.
        if (_SCRegistry == null) return null;

        foreach (SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.ID.Equals(id)) return item;
        }
        return null;
    }

    //Returns all connector data items with the from the given scene name.
    public static List<SceneConnector.SceneConnectorData> GetConnectorsInScene(string sceneName)
    {
        List<SceneConnector.SceneConnectorData> items = new List<SceneConnector.SceneConnectorData>();

        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return null.
        if (_SCRegistry == null) return null;

        foreach (SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.sceneName.Equals(sceneName)) items.Add(item);
        }

        if (items.Count == 0) return null;
        return items;
    }

    public static string RegistryToString(List<SceneConnector.SceneConnectorData> scRegistry)
    {
        string str = "";

        //Loop through each SceneConnectorRegistry_Data item, convert them to strings and append them to a final string, then return
        foreach(SceneConnector.SceneConnectorData item in scRegistry)
        {
            str += item.ToString() + "|";
        }
        return str;
    }

    public static List<SceneConnector.SceneConnectorData> FromString(string str)
    {
        string s = "";
        _SCRegistry = new List<SceneConnector.SceneConnectorData>();
        foreach (char c in str)
        {
            if (c.Equals('|'))
            {
                SceneConnector.SceneConnectorData data = new SceneConnector.SceneConnectorData() ;
                data.FromString(s);
                _SCRegistry.Add(data);
                
                s = "";
            }
            else s += c;
        }
        return _SCRegistry;
    }

    //Sets the destination scene name in the registry data for the given id, if it exists.
    public static void SetDestinationAtID(string id, string destinationSceneName, string destinationID, string destinationConnectorName)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return.
        if (_SCRegistry == null) { if (bDebug) Debug.Log("Scene Connector Registry [Set Destination] : No Registry Found"); return; }

        foreach (SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.ID.Equals(id)) 
            {
                item.destinationSceneName = destinationSceneName; 
                item.destinationSceneID = destinationID;
                item.destinationConnectorName = destinationConnectorName;
            }
        }

        //Update saved registry in persistent data persistent data.
        SaveSystem.StaticSaveString(_SCRegistryKey, RegistryToString(_SCRegistry));
    }

    //Removes the destination information from the item with the given ID.
    public static void RemoveDestinationAtID(string id)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return.
        if (_SCRegistry == null) { if (bDebug) Debug.Log("Scene Connector Registry [Set Destination] : No Registry Found"); return; }

        foreach (SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.ID.Equals(id)) 
            {
                item.destinationSceneName = ""; 
                item.destinationSceneID = "";
                item.destinationConnectorName = "";
            }
        }

        //Update saved registry in persistent data persistent data.
        SaveSystem.StaticSaveString(_SCRegistryKey, RegistryToString(_SCRegistry));
    }

    //Removes all destination information from items with the given destinationID
    public static void RemoveAllDestinationInfoWithDestinationID(string destinationID)
    {
        //Get registry from persistant data.
        _SCRegistry = GetRegistry();

        //If it doesnt exist return.
        if (_SCRegistry == null) { if (bDebug) Debug.Log("Scene Connector Registry [Set Destination] : No Registry Found"); return; }

        foreach (SceneConnector.SceneConnectorData item in _SCRegistry)
        {
            if (item.destinationSceneID.Equals(destinationID))
            {
                item.destinationSceneName = ""; 
                item.destinationSceneID = "";
                item.destinationConnectorName = "";
            }
        }

        //Update saved registry in persistent data persistent data.
        SaveSystem.StaticSaveString(_SCRegistryKey, RegistryToString(_SCRegistry));
    }

    
}
