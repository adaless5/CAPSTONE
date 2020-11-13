using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameSettingsEditorWindow : EditorWindow
{
    bool _showGeneralSettings = true;
    bool _exportConnector = false;
    bool _clearAllFoldout = false;

    [MenuItem("Window/GameSettings")]
    static void ShowWindow()
    {
        GetWindow<GameSettingsEditorWindow>("GameSettings");
    }

    //Handles the drawing of the Game Settings editor window.
    void OnGUI()
    {
        _showGeneralSettings = EditorGUILayout.Foldout(_showGeneralSettings, "General Settings");

        if (_showGeneralSettings)
        {

            if (GUILayout.Button("Reset Saved Data"))
            {
                DefaultIDRegistry.DeleteAll();
            }

            if (GUILayout.Button("Delete Respawn Info"))
            {
                PlayerPrefs.DeleteKey(SaveSystem.RESPAWN_INFO_REGISTRY_ID);
                Debug.Log("Deleted Respawn Info");
            }

            DrawUILine(Color.grey);
            GUILayout.Label("Import/Export Scene Connector Data");

            if (GUILayout.Button("Import"))
            {
                SceneConnector.ImportConnectorDataFromText();
            }

            _exportConnector = EditorGUILayout.Foldout(_exportConnector, "Export Connectors ( Import first! )");
            if (_exportConnector)
            {
                if (GUILayout.Button("Export"))
                {
                    SceneConnector.ExportConnectorDataFromText();
                }
            }

            _clearAllFoldout = EditorGUILayout.Foldout(_clearAllFoldout, "Clear All Data [Dangerous]");
            if (_clearAllFoldout)
            {
                if (GUILayout.Button("Delete ALL Saved Data"))
                {
                    PlayerPrefs.DeleteAll();
                    Debug.Log("ALL SAVED DATA DELETED");
                }
            }
        }
    }

    //This runs everytime the editor window changes.
    void OnInspectorUpdate()
    {
        Repaint();
    }

    //Draws a simple UI line.
    private static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
