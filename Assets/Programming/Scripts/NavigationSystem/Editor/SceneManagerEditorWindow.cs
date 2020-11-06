using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Rendering;
using System.Collections.ObjectModel;
using UnityEditor.SceneManagement;
using System;

//Custom Scene Manager Editor Window
public class SceneManagerEditorWindow : EditorWindow
{
    DirectoryInfo[] _levels;
    bool[] _levelIsSplit;
    List<FileInfo[]> _scenes;

    bool[] _levelFoldouts;
    Dictionary<int,bool[]> _sceneFoldouts;
    Dictionary<int, bool[]> _bSceneFoundInBuild;
    
    float MINI_WINDOW_SIZE = 220f;
    const float SMALL_WINDOW_SIZE = 430f;
    const float MEDIUM_WINDOW_SIZE = 570f;

    bool bDeleteMode = false;
    int activeSceneAtLevelIndex = -1;

    Vector2 _scrollPosition;

    [MenuItem("Window/Scene Manager")]
    static void ShowWindow()
    {
        GetWindow<SceneManagerEditorWindow>("Scene Manager");
    }

    private void OnEnable()
    {
        FetchLevelsAndScenesFromSceneFolder();
        PopulateLevelAndSceneFoldoutBools();
    }

    private void OnGUI()
    {
        GUI.backgroundColor = Color.grey;

        //Open all Foldouts button
        if (GUI.Button(new Rect(10, 5, 25, 20), "v", EditorStyles.miniButton))
        {
            ToggleAllFoldouts(true);
        }
        //

        //Close all foldouts button
        if (GUI.Button(new Rect(45, 5, 25, 20), ">", EditorStyles.miniButton))
        {
            ToggleAllFoldouts(false);
        }
        //

        //Toggle Delete Mode Button
        if (bDeleteMode) GUI.backgroundColor = Color.red; else GUI.backgroundColor = Color.grey;
        if (GUI.Button(new Rect(80, 5, 50, 20), "del", EditorStyles.miniButton))
        {
            ToggleDeleteMode();
        }
        GUI.backgroundColor = Color.grey;
        //

        //Refresh Scenes Button
        if (GUI.Button(new Rect(Screen.width - 90f, 5, 80, 20),"Refresh",EditorStyles.miniButton))
        {
            FetchLevelsAndScenesFromSceneFolder();
            PopulateLevelAndSceneFoldoutBools();
        }
        GUILayout.Space(30);
        //

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        GUILayout.Label("Levels / Scenes / Connectors", EditorStyles.boldLabel);
        if (_levels != null)
        {
            DrawUILine(Color.white);

            //LEVELS
            for (int levelIndex = 0; levelIndex < _levels.Length; levelIndex++)
            {
                //Check for Split level modifier and set appropriate color.
                GUI.contentColor = SetLevelColorAndIsSplit(levelIndex);

                //Display Levels
                _levelFoldouts[levelIndex] = 
                    EditorGUILayout.Foldout(_levelFoldouts[levelIndex], GetLevelNameWithoutModifier(_levels[levelIndex].Name));
                //

                GUI.contentColor = Color.white;

                if (_levelFoldouts[levelIndex])
                {   
                    //SCENES
                    for (int sceneIndex = 0; sceneIndex < _sceneFoldouts[levelIndex].Length; sceneIndex++)
                    {
                        //Display Scenes
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        GUI.contentColor = SceneFoundInBuildSettings(levelIndex,sceneIndex);
                        _sceneFoldouts[levelIndex][sceneIndex] =
                            EditorGUILayout.Foldout(_sceneFoldouts[levelIndex][sceneIndex], GetFileNameWithoutExtention(_scenes[levelIndex][sceneIndex].Name));
                        GUI.contentColor = Color.white;
                        GUILayout.Space(Screen.width*.5f);
                        //

                        //Set active Scene at Level Index
                        if (SceneManager.GetActiveScene().name.Equals(GetFileNameWithoutExtention(_scenes[levelIndex][sceneIndex].Name))
                            && SceneManager.sceneCount == 1)
                        {
                            activeSceneAtLevelIndex = levelIndex;
                        }
                        //

                        //Change Load Scene Button To Green If scene is currently loaded
                        GUI.backgroundColor = SetLoadButtonColor(sceneIndex,levelIndex);

                        //Display Load Scene Button
                        if(Screen.width >= MINI_WINDOW_SIZE)
                        {
                            //Display Add Scene if currently loaded scene is in a split level
                            if (activeSceneAtLevelIndex == levelIndex && _levelIsSplit[levelIndex])
                            {
                                if (GUI.backgroundColor == Color.grey)
                                {
                                    //Add Scene Additively if level is a split level
                                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft))
                                    {
                                        try { EditorSceneManager.OpenScene("Assets/Scenes/" + _levels[levelIndex].Name + "/" + _scenes[levelIndex][sceneIndex].Name, OpenSceneMode.Additive); }
                                        catch { Debug.Log("Scene Manager : Scene not found"); }
                                    }
                                }
                                else
                                {
                                    //Remove Scene if it is a split scene, the scene is loaded and there are atleast 2 scenes open.
                                    string s = "+";
                                    if (SceneManager.sceneCount > 1) s = "-";
                                    if (GUILayout.Button(s, EditorStyles.miniButtonLeft))
                                    {
                                        //Save All open Scenes to prevent data corruption between scene loads.
                                        SaveOpenScenes();
                                        //

                                        try { EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(
                                            "Assets/Scenes/" + _levels[levelIndex].Name + "/" + _scenes[levelIndex][sceneIndex].Name),true);
                                            }
                                        catch { Debug.Log("Scene Manager : Scene not found"); }
                                    }
                                }
                            }
                            else
                            {
                                //Else Display Load Scene Button
                                if (GUILayout.Button("Load", EditorStyles.miniButtonLeft))
                                {
                                    SaveOpenScenes();

                                    try { EditorSceneManager.OpenScene("Assets/Scenes/" + _levels[levelIndex].Name + "/" + _scenes[levelIndex][sceneIndex].Name); }
                                    catch { Debug.Log("Scene Manager : Scene not found"); }
                                }
                            }
                        }
                        GUI.backgroundColor = Color.grey;
                        GUILayout.EndHorizontal();
                        //

                        //CONNECTORS
                        if (_sceneFoldouts[levelIndex][sceneIndex])
                        {
                            List<SceneConnector.SceneConnectorData> connectors
                                = SceneConnectorRegistry.GetConnectorsInScene(GetFileNameWithoutExtention(_scenes[levelIndex][sceneIndex].Name));

                            if (connectors != null)
                            {
                                //Display Connector information bar if there are connectors in the scene.
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                if (Screen.width < SMALL_WINDOW_SIZE) GUILayout.Label("Connector Name" + "\t\t" + "ID");
                                else if (Screen.width < MEDIUM_WINDOW_SIZE) GUILayout.Label("Connector Name" + "\t\t" + "ID" + "\t ->  " + "Destination Scene" + "\t" + "ID");
                                else  GUILayout.Label("Connector Name" + "\t\t" + "ID" + "\t ->  " + "Destination Scene" + "\t" + "Connector Name" + "\t\t" + "ID");
                                GUILayout.EndHorizontal();
                                DrawUILine(Color.black,1,0,40);
                                //

                                foreach (SceneConnector.SceneConnectorData connector in connectors)
                                {
                                    //Display Connector Data
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(16);

                                    if(bDeleteMode)
                                    {
                                        GUI.backgroundColor = Color.red;
                                        if (GUILayout.Button("d", GUILayout.Width(17f)))
                                        {
                                            SceneConnectorRegistry.Remove(connector.ID);
                                            SceneConnectorRegistry.RemoveAllDestinationInfoWithDestinationID(connector.ID);
                                            SaveSystem.RemoveAtKey(connector.name + connector.sceneName,"");
                                        }
                                        GUI.backgroundColor = Color.grey;
                                    }
                                    else
                                    {
                                        if (GUILayout.Button("c", GUILayout.Width(17f)))
                                        {
                                            GUIUtility.systemCopyBuffer = connector.ID;
                                            Debug.Log("ID Copied To Clipboard");
                                        }
                                    }
                                    
                                    GUILayout.Space(4);
                                    if (Screen.width < SMALL_WINDOW_SIZE) GUILayout.Label(Resized(connector.name) + "\t\t" + connector.ID);
                                    else if (Screen.width < MEDIUM_WINDOW_SIZE) GUILayout.Label(Resized(connector.name) + "\t\t" + connector.ID + "\t ->  " + Resized(connector.destinationSceneName) + "\t\t" + connector.destinationSceneID);
                                    else GUILayout.Label(Resized(connector.name) + "\t\t" + connector.ID + "\t ->  " + Resized(connector.destinationSceneName) + "\t\t" + Resized(connector.destinationConnectorName) + "\t\t" + connector.destinationSceneID);
                                    GUILayout.EndHorizontal();
                                    //
                                }
                            }
                        }
                    }
                }
                DrawUILine(Color.grey);
            }
        }

        //Display Scene not in build folder if any are not in the build folder
        if (!AllScenesFoundInBuildSettings())
        {
            GUILayout.TextArea("Scenes with a red name are not found in the build settings. Please add them here : File->Build Settings ");
        }
        //
        GUILayout.EndScrollView();
    }

    private void SaveOpenScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            EditorSceneManager.SaveScene(SceneManager.GetSceneAt(i));
        }
    }

    //Set load button color to green if the scene is currently loaded. else set it to grey.
    private Color SetLoadButtonColor(int sceneIndex, int levelIndex)
    {
        if (SceneManager.GetSceneByName(GetFileNameWithoutExtention(_scenes[levelIndex][sceneIndex].Name)).IsValid()) return Color.green;
        return Color.grey;
    }

    //Change level color depending on type. a normal level is white, a split level (defined by _s at the end of the directory name) is baby blue.
    //Also toggle level bool to true if its a split level.
    private Color SetLevelColorAndIsSplit(int levelIndex)
    {
        string levelName = _levels[levelIndex].Name;

        //Set if level is split
        _levelIsSplit[levelIndex] = levelName[levelName.Length - 2].Equals('_') && levelName[levelName.Length - 1].Equals('s');
        //

        //Return appropriate color depending on level type.
        if (_levelIsSplit[levelIndex]) return Color.cyan;
        return Color.white;
        //
    }

    //Returns the name of the level without any modifiers (_s)
    private string GetLevelNameWithoutModifier(string levelName)
    {
        //Clip Modifer from name if it exists
        if (levelName[levelName.Length - 2].Equals('_') && levelName[levelName.Length - 1].Equals('s'))
        {
            string s = "";
            for (int i = 0; i < levelName.Length - 2; i++) s += levelName[i];
            return s;
        }
        return levelName;
    }

    private void OnInspectorUpdate()
    {
        //Repaint window so it updates even when the cursor is not active on the window.
        Repaint();
    }

    //Fetches the Levels and their included scenes from the Scene Directory.
    private void FetchLevelsAndScenesFromSceneFolder()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Scenes");
        _levels = dir.GetDirectories();
        _levelIsSplit = new bool[_levels.Length];

        _scenes = new List<FileInfo[]>();
        foreach (DirectoryInfo level in _levels)
        {
            _scenes.Add(level.GetFiles("*.unity"));
        }
    }

    //Populates foldout values for each level and scene found in the Scene Directory.
    private void PopulateLevelAndSceneFoldoutBools()
    {
        _levelFoldouts = new bool[_levels.Length];
        for (int i = 0; i < _levelFoldouts.Length; i++) _levelFoldouts[i] = true; //Default levels open to show scenes.

        _sceneFoldouts = new Dictionary<int, bool[]>();
        _bSceneFoundInBuild = new Dictionary<int, bool[]>();

        for (int i = 0; i < _levels.Length; i++)
        {
            _sceneFoldouts.Add(i, new bool[_levels[i].GetFiles("*.unity").Length]);
            _bSceneFoundInBuild.Add(i, new bool[_levels[i].GetFiles("*.unity").Length]);
        }
    }

    //Returns the file name without the extention
    private string GetFileNameWithoutExtention(string fileName)
    {
        string s = "";
        foreach(char c in fileName)
        {
            if (c.Equals('.')) return s;
            s += c;
        }
        return s;
    }

    //Toggle all foldouts to open/close
    private void ToggleAllFoldouts(bool toggle)
    {
        for (int i = 0; i < _levelFoldouts.Length; i++) _levelFoldouts[i]= toggle;

        foreach (KeyValuePair<int,bool[]> item in _sceneFoldouts)
        {
            for (int i = 0; i < item.Value.Length; i++) item.Value[i] = toggle;
        }
    }

    //Toggles Delete Mode : used to remove connectors manually from the registry in the scene manager window.
    private void ToggleDeleteMode()
    {
        if (bDeleteMode) bDeleteMode = false;
        else bDeleteMode = true;
    }

    //Returns a color if the scene is found in the build settings. 
    //used to color the scene names according to their status in the build settings.
    private Color SceneFoundInBuildSettings(int levelIndex, int sceneIndex)
    {
        string path = "Assets/Scenes/" + _levels[levelIndex].Name + "/" + _scenes[levelIndex][sceneIndex].Name;

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path.Equals(path))
            {
                _bSceneFoundInBuild[levelIndex][sceneIndex] = true;
                return Color.green;
            }
        }
        _bSceneFoundInBuild[levelIndex][sceneIndex] = false;
        return Color.red;
    }

    //Returns true if all scenes exist in the build settings. Used to warn the player to add them if any are missing.
    private bool AllScenesFoundInBuildSettings()
    {
        foreach (KeyValuePair<int, bool[]> item in _bSceneFoundInBuild)
        {
            for (int i = 0; i < item.Value.Length; i++)
            {
                if (item.Value[i] == false) return false;
            }
        }
        return true;
    }

    //Draw UI line for this window (unique to this window)
    private static void DrawUILine(Color color, int thickness = 1, int padding = 10, int xPadding = 0)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x = xPadding;

        r.width = Screen.width - 5f;
        EditorGUI.DrawRect(r, color);
    }

    //Returns the trimmed string if it exceeds the max size.
    const int MAX_STRING_SIZE = 12;
    private string Resized(string str)
    {
        //Add white spaces if string is too short.
        if (str.Length < MAX_STRING_SIZE) 
        {
            string s = "";
            for (int i = 0; i < MAX_STRING_SIZE - str.Length; i++) s += " ";
            return str + s;
        }

        //Trim String if its too long
        string ss = ""; 
        for (int i = 0; i < MAX_STRING_SIZE; i++) ss += str[i];
        return ss + "..";
    }
}
