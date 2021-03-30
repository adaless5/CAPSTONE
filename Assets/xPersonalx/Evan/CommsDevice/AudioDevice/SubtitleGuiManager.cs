using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Controls all the Gui Elements
public class SubtitleGuiManager : MonoBehaviour
{
    public Text textBox;

    public void SetText(string text)
    {
        textBox.text = text;
    }

    public void Clear()
    {
        textBox.text = string.Empty;
    }
}
