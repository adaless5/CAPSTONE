
using UnityEngine;
using UnityEditor;

public class GameSettingsEditorWindow : EditorWindow
{
    bool _showPlayerSettings = true;

    //Player Setting members.
    KeyCode _jumpKey = GameSettings.jumpKey;
    float _jumpMultiplier = GameSettings.jumpMultiplier;
    KeyCode _runKey = GameSettings.runKey;
    float _walkSpeed = GameSettings.walkSpeed;
    float _runSpeed = GameSettings.runSpeed;
    float _runBuildupMultiplier = GameSettings.runBuildupMultiplier;
    float _lookSensitivity = GameSettings.lookSensitivity;
    //

    [MenuItem("Window/GameSettings")]
    static void ShowWindow()
    {
        GetWindow<GameSettingsEditorWindow>("GameSettings");
    }

    //Handles the drawing of the Game Settings editor window.
    void OnGUI()
    {
        _showPlayerSettings = EditorGUILayout.Foldout(_showPlayerSettings, "Player Settings");

        if (_showPlayerSettings)
        {
            DrawUILine(Color.grey);

            ////Jump Settings
            EditorGUILayout.LabelField("Jump Settings", EditorStyles.boldLabel);

            //Set Jump Key
            _jumpKey = (KeyCode)EditorGUILayout.EnumPopup("    Jump Key:", _jumpKey);

            //Set Jump multiplier
            _jumpMultiplier = EditorGUILayout.Slider("    Jump Height:", _jumpMultiplier, 1.0f, 20.0f);
            ////

            DrawUILine(Color.grey);

            ////Movement Settings
            EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);

            //Set Run Key
            _runKey = (KeyCode)EditorGUILayout.EnumPopup("    Run Key:", _runKey);

            //Set Walk / Run Speed
            EditorGUILayout.LabelField("    Walking Speed:  " + _walkSpeed.ToString(".0") + "    Running Speed:  " + _runSpeed.ToString(".0"));
            EditorGUILayout.MinMaxSlider(ref _walkSpeed, ref _runSpeed, 1.0f, 15.0f);

            //Set Run Buildup Speed
            _runBuildupMultiplier = EditorGUILayout.Slider("    Run Buildup Speed:", _runBuildupMultiplier, 1.0f, 5.0f);

            //Set Look Sensitivity
            _lookSensitivity = EditorGUILayout.Slider("    Look Sensitivity:", _lookSensitivity, 100.0f, 600.0f);
            ////

            DrawUILine(Color.grey);

            if (GUILayout.Button("Reset To Default"))
            {
                GameSettings.ResetToDefault("Player");

                //Update values in editor.
                _jumpKey = GameSettings.jumpKey;
                _jumpMultiplier = GameSettings.jumpMultiplier;
                _runKey = GameSettings.runKey;
                _walkSpeed = GameSettings.walkSpeed;
                _runSpeed = GameSettings.runSpeed;
                _runBuildupMultiplier = GameSettings.runBuildupMultiplier;
                _lookSensitivity = GameSettings.lookSensitivity;
            }
        }
    }

    //This runs everytime the editor window changes.
    void OnInspectorUpdate()
    {
        this.Repaint();
        SavePlayerSettings();
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

    //Saves the Player Settings using Editor Prefs. this allows data to persist through rebuilds.
    private void SavePlayerSettings()
    {
        EditorPrefs.SetString("GameSettings_jumpKey", _jumpKey.ToString());
        EditorPrefs.SetFloat("GameSettings_jumpMultiplier", _jumpMultiplier);
        EditorPrefs.SetString("GameSettings_runKey", _runKey.ToString());
        EditorPrefs.SetFloat("GameSettings_walkSpeed", _walkSpeed);
        EditorPrefs.SetFloat("GameSettings_runSpeed", _runSpeed);
        EditorPrefs.SetFloat("GameSettings_runBuildUpMultiplier", _runBuildupMultiplier);
        EditorPrefs.SetFloat("GameSettings_lookSensitivity", _lookSensitivity);
    }

}
