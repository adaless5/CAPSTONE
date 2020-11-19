using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Runtime.InteropServices;
using System;

//Custom Inspector Editor for the Scene Connector.
[CustomEditor(typeof(SceneConnector))]
public class SceneConnectorEditor : Editor
{
    bool bDebug = false;
    bool bConnectorMatchesSavedData = false;
    bool bFinalized = false;

    SceneConnector _base;
    string _sceneName;
    SceneConnector.SceneConnectorData _data;
    SceneConnector.SceneConnectorType _type;

    string _goesToID = "";
    string _goesToScene = "";
    string _ID = "";

    //Runs when the custom component becomes active in the inspector
    void OnEnable()
    {
        //Initialize members;
        _base = (SceneConnector)target;
        _sceneName = _base.gameObject.scene.name;
        _data = new SceneConnector.SceneConnectorData(_base.transform, "", _sceneName, target.name, _type, "", "");

        //LOAD DATA
        _data.FromString(SaveSystem.LoadString(target.name + _sceneName, "",_sceneName));
        if (bDebug) Debug.Log("LOADED : " + target.name + _data.ToString());

        //Synchronize members with _data
        _ID = _data.ID;
        _goesToID = _data.destinationSceneID;
        _goesToScene = _data.destinationSceneName;
        _type = _data.type;
        
    }

    //Handles the drawing of the Custom Inspector.
    public override void OnInspectorGUI()
    {
        //Padding Info for ui
        float vPadding = 3;
        float hPadding = 10;
        float columnWidth = Screen.width / 5;
        //

        //Synchronize members with _base.
        _base._ID = _ID;
        _base._destinationSceneID = _goesToID;
        _base._destinationSceneName = _goesToScene;
        _base._type = _type;
        //

        //ID INFORMATION
        GUI.contentColor = Color.white;
        GUI.Label(new Rect(columnWidth / 2, vPadding, columnWidth - hPadding, 20), "ID : ");

        if (IDisBlank()) GUI.backgroundColor = Color.red;
        else GUI.backgroundColor = Color.green;

        GUI.Box(new Rect(hPadding + columnWidth, vPadding, columnWidth * 2 - hPadding, 20), _ID);
        GUI.backgroundColor = Color.grey;

        //Generate button
        if (IDisBlank())
        {
            if (GUI.Button(new Rect(hPadding + columnWidth * vPadding, vPadding, columnWidth * 2 - hPadding * 2, 20), "Generate"))
            {
                _ID = IDGenerator.GenerateUniqueID();
                SaveData();
            }
        }
        else //Copy Button
        {
            if (GUI.Button(new Rect(hPadding + columnWidth * vPadding, vPadding, columnWidth * 2 - hPadding * 2, 20), "Copy"))
            {
                GUIUtility.systemCopyBuffer = _ID;
                if (bDebug) Debug.Log("Scene Connector : ID Copied To Clipboard");
            }
        }
        GUILayout.Space(20 + vPadding);

        //Check for changes in save data, if data is dirty and needs to be updated, then remove it from the registry
        if (!SceneConnectorMatchesSavedData())
        {
            GUI.backgroundColor = Color.red;
            if (bConnectorMatchesSavedData)
            {
                RemoveData();
                bConnectorMatchesSavedData = false;
            }
        }
        else
        {
            GUI.backgroundColor = Color.green;
            bConnectorMatchesSavedData = true;
        }

        //Save ID Button
        if (GUILayout.Button("Save ID"))
        {
            if (!IDisBlank()) SaveData();
        }
        GUI.backgroundColor = Color.grey;
        //

        DrawUILine(Color.grey);
        GUI.backgroundColor = Color.grey;

        //TYPE INFORMATION
        GUILayout.Label("TYPE INFORMATION", EditorStyles.boldLabel);
        _type = (SceneConnector.SceneConnectorType)EditorGUILayout.EnumPopup("Connector Type: ", _type);

        ///Show Special Buttons depending on Connector Type
        //Orient Button for Non Euclidian Connectors
        if (_type == SceneConnector.SceneConnectorType.NonEuclidian)
        {
            GUILayout.Space(3.0f);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Orient"))
            {
                //TODO: Orient scene
            }
            GUI.backgroundColor = Color.grey;
        }

        //Generate Unload Trigger for Seamless Connectors
        else if (_type == SceneConnector.SceneConnectorType.Seamless)
        {
            GUILayout.Space(3.0f);
            GUI.backgroundColor = SceneConnectorContainsUnloadTrigger();
            if (GUILayout.Button("Generate Unload Trigger"))
            {
                if(GUI.backgroundColor.Equals(Color.red)) _base.CreateUnloadTrigger();
                SaveData();
            }
            GUI.backgroundColor = Color.grey;
        }
        ///
        DrawUILine(Color.grey);

        //DESTINATION INFORMATION
        GUILayout.Label("DESTINATION INFORMATION", EditorStyles.boldLabel);

        GUILayout.Space(2);

        //Add Voffset in case a special button is present, to lower the following elements
        int vOffset;
        if (_type == SceneConnector.SceneConnectorType.NonEuclidian || _type == SceneConnector.SceneConnectorType.Seamless) vOffset = 23;
        else vOffset = 0;

        //Show Destination Information if Scene Connector type is Non Euclid or Portal
        if (_type == SceneConnector.SceneConnectorType.NonEuclidian || _type == SceneConnector.SceneConnectorType.Portal)
        {
            GUILayout.Label("Goes To ID : ");

            //Go To Id Color Confirmations
            if (!SceneConnectorExistsInRegistry(_goesToID) || DestinationIDMatchesThisID(_goesToID)
                || IsNonEuclidOrSeamlessAndDestinationSceneMatchesThisScene(_goesToID))
            {
                GUI.backgroundColor = Color.red;

                //If information changed after object is finalized, remove it from the registry. The data no longer matches saved memory.
                if (bFinalized)
                {
                    bFinalized = false;
                    SceneConnectorRegistry.RemoveDestinationAtID(_ID);

                    Debug.Log("Destination : " + SceneConnectorRegistry.GetDataFromID(_ID).destinationSceneName);
                }
            }
            else GUI.backgroundColor = Color.green;
            //

            // Go To ID
            _goesToID = GUI.TextField(new Rect(100, 130 + vOffset, Screen.width - 110, 20), _goesToID);
            //

            GUI.backgroundColor = Color.grey;

            ////Goes to ID Restriction checks
            if (DestinationIDMatchesThisID(_goesToID) && !IDisBlank()) GUILayout.TextArea("Scene Connector can not connect to itself");
            else if (IsNonEuclidOrSeamlessAndDestinationSceneMatchesThisScene(_goesToID)) GUILayout.TextArea("Non-Euclidian and Seamless connectors must connect to a different scene");
            ////
            else if (SceneConnectorExistsInRegistry(_goesToID))
            {
                GUILayout.Space(3.0f);
                SceneConnector.SceneConnectorData goToData = SceneConnectorRegistry.GetDataFromID(_goesToID);

                GUILayout.Label("In Scene : " + goToData.sceneName);
                GUILayout.Label("At Connector : " + goToData.name);

                GUILayout.Space(3.0f);

                //Finalize Color confirmations
                if (!DestinationSceneNameMatchesSavedDataSceneName(goToData.sceneName) || !SceneConnectorMatchesSavedData())
                {
                    GUI.backgroundColor = Color.red;
                }
                else GUI.backgroundColor = Color.green;
                //

                if (GUILayout.Button("Finalize"))
                {
                    bFinalized = true;

                    SceneConnectorRegistry.SetDestinationAtID(_ID, goToData.sceneName, goToData.ID, goToData.name);
                    _goesToID = goToData.ID;
                    _goesToScene = goToData.sceneName;
                    SaveData(goToData.name);
                }
            }
        }
        //Show Destination Information if Scene Connector type is Seamless
        else if (_type == SceneConnector.SceneConnectorType.Seamless)
        {
            GUILayout.Label("Goes To Scene : ");

            if (DestinationSceneNameMatchesSavedDataSceneName(_goesToScene) && _goesToScene != "") GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.red;

            // Go To Scene
            _goesToScene = GUI.TextField(new Rect(120, 130 + vOffset, Screen.width - 130, 20), _goesToScene);
            //

            if (SceneExists(_goesToScene))
            {
                GUILayout.Space(3.0f);

                if (GUILayout.Button("Finalize"))
                {
                    bFinalized = true;

                    SceneConnectorRegistry.SetDestinationAtID(_ID, _goesToScene, "", "");
                    _base._destinationSceneName = _goesToScene;
                    SaveData();
                }
            }
            GUI.backgroundColor = Color.grey;
        }

        //Check if scene is not seamless and has an unload trigger then delete it.
        if (_type != SceneConnector.SceneConnectorType.Seamless)
        {
            if (SceneConnectorContainsUnloadTrigger().Equals(Color.green)) _base.DeleteUnloadTrigger();
        }
    }

    //Returns True if scene exists in build settings
    private bool SceneExists(string scene)
    {
        //TODO: Check if scene is in the same level.
        return SceneManager.GetSceneByName(scene).IsValid();
    }

    //Returns true if stored data for the connector matches the current state of the connector.
    private bool SceneConnectorMatchesSavedData()
    {
        Transform t = _base.GetComponentInChildren<PlayerStart>().transform;

        return (QualityOfLifeFunctions.CloseEnough(_data.pos,t.position) 
            && QualityOfLifeFunctions.CloseEnough(_data.rot, t.rotation)
            && _ID.Equals(_data.ID) && _data.name == target.name
            && !SceneConnectorRegistry.IsEmpty()) ? true : false;
    }

    private Color SceneConnectorContainsUnloadTrigger()
    {
        UnloadTrigger trig = _base.GetComponentInChildren<UnloadTrigger>();
        if (trig == null) return Color.red;
        return Color.green;
    }

    //Returns true if ID is blank.
    private bool IDisBlank()
    {
        return _ID == "" ? true : false;
    }

    //Returns true if the connector exists within the scene registry.
    private bool SceneConnectorExistsInRegistry(string id)
    {
        return SceneConnectorRegistry.Contains(id);
    }

    //Returns true if the destination ID entered matches the connectors ID. Test for connector trying to connect to itself.
    private bool DestinationIDMatchesThisID(string destinationID)
    {
        return destinationID.Equals(_ID);
    }

    //Returns true if the destination sceneName entered matches the connectors sceneName.
    private bool DestinationSceneNameMatchesSavedDataSceneName(string destinationSceneName)
    {
        try { return destinationSceneName == SceneConnectorRegistry.GetDataFromID(_ID).destinationSceneName; }   
        catch {return false;} // return false if ID not found in registry
    }

    //Returns true if connector is seamless or non euclid and the destination connector is in the same scene.
    //This is a test, as the way these connectors work this is impossible.
    private bool IsNonEuclidOrSeamlessAndDestinationSceneMatchesThisScene(string destinationID)
    {
        try
        {
            return ((_type == SceneConnector.SceneConnectorType.NonEuclidian || _base._type == SceneConnector.SceneConnectorType.Seamless) // 
                &&  _sceneName == SceneConnectorRegistry.GetDataFromID(_goesToID).sceneName);
        }
        catch { return false; }
    }

    //Saves the necessary connector information to the connector registry.
    private void SaveData(string goToConnectorName = "")
    {
        //Save Connector to persistant data
        _data = new SceneConnector.SceneConnectorData(_base.GetComponentInChildren<PlayerStart>().transform, _ID, _sceneName, target.name, _type, _goesToID, _goesToScene);
        _data.destinationConnectorName = goToConnectorName;
        SaveSystem.Save(target.name + _sceneName,"",_sceneName, _data.ToString(),SaveSystem.SaveType.CONNECTOR);
        if(bDebug)Debug.Log("SAVED : " + target.name + _data.ToString() );

        //Save Connector data to SceneConnectorRegistry
        if (!SceneConnectorRegistry.Contains(_ID))
            SceneConnectorRegistry.Add(_data);
    }

    //Saves the necessary connector information to the connector registry.
    private void RemoveData()
    {
        SceneConnectorRegistry.Remove(_ID);
        SceneConnectorRegistry.RemoveAllDestinationInfoWithDestinationID(_ID);
        SaveSystem.RemoveAtKey(target.name + _sceneName, "",_sceneName);
    }

    //This runs everytime the editor window changes.


    //Draw UI line. (Unique to this editor)
    private static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;

        r.width = Screen.width - padding * 3.5f;
        EditorGUI.DrawRect(r, color);
    }
}
