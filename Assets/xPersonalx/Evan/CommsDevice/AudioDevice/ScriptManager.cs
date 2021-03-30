using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScriptManager : MonoBehaviour
{
    //going to create a key value pair where both the key and value are strings
    //but when comparing strings, ignore the case
    private Dictionary<string, string[]> lines = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

    private string resourceFile = "LoreScript";

    private void Awake()
    {
        //loading in text asset
        var textAsset = Resources.Load<TextAsset>(resourceFile);
        //taking text asset and parsing it
        var voText = JsonUtility.FromJson<VoiceOverText>(textAsset.text);
        foreach (var t in voText.lines)
        {
            lines[t.key] = t.line;
        }
    }


    public string[] GetText(string textKey)
    {
        string[] tmp = new string[] { };
        if (lines.TryGetValue(textKey, out tmp))
        {
            return tmp;
        }
        return new string[] { "<color=#ff00ff>MISSING TEXT FOR '" + textKey + "'</color>" };
    }
}
