using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameSettings : MonoBehaviour
{
    ////Player Settings
    //Defaults
    const KeyCode JUMP_KEY_DEFAULT = KeyCode.Space;
    const float JUMP_MULTIPLIER_DEFAULT = 10.0f;
    const KeyCode RUN_KEY_DEFAULT = KeyCode.LeftShift;
    const float WALK_SPEED_DEFAULT = 6.5f;
    const float RUN_SPEED_DEFAULT = 11.0f;
    const float RUN_BUILDUP_MULTIPLIER_DEFAULT = 4.0f;
    const float LOOK_SENSITIVITY_DEFAULT = 350.0f;
    //

    static public KeyCode jumpKey { get; set; }
    static public float jumpMultiplier { get; set; }
    static public KeyCode runKey { get; set; }
    static public float walkSpeed { get; set; }
    static public float runSpeed { get; set; }
    static public float runBuildupMultiplier { get; set; }
    static public float lookSensitivity { get; set; }
    ////


    void Awake()
    {
        //Retrieve GameSettings Editor Data.
        jumpKey = StringToKeyCodeHelper.StringToKeyCode(EditorPrefs.GetString("GameSettings_jumpKey"));
        jumpMultiplier = EditorPrefs.GetFloat("GameSettings_jumpMultiplier");
        runKey = StringToKeyCodeHelper.StringToKeyCode(EditorPrefs.GetString("GameSettings_runKey"));
        walkSpeed = EditorPrefs.GetFloat("GameSettings_walkSpeed");
        runSpeed = EditorPrefs.GetFloat("GameSettings_runSpeed");
        runBuildupMultiplier = EditorPrefs.GetFloat("GameSettings_runBuildUpMultiplier");
        lookSensitivity = EditorPrefs.GetFloat("GameSettings_lookSensitivity");
    }

    //Handles reseting Game Settings
    static public void ResetToDefault(string tag)
    {
        if (tag == "Player") ResetPlayerSettingsToDefault();
    }

    //Resets Player Settings to their default values;
    static void ResetPlayerSettingsToDefault()
    {
        jumpKey = JUMP_KEY_DEFAULT;
        jumpMultiplier = JUMP_MULTIPLIER_DEFAULT;
        runKey = RUN_KEY_DEFAULT;
        walkSpeed = WALK_SPEED_DEFAULT;
        runSpeed = RUN_SPEED_DEFAULT;
        runBuildupMultiplier = RUN_BUILDUP_MULTIPLIER_DEFAULT;
        lookSensitivity = LOOK_SENSITIVITY_DEFAULT;
    }
}
