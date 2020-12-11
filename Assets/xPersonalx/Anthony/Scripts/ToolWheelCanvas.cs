using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolWheelCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ActivateButton[] _WheelButtons = GetComponentsInChildren<ActivateButton>();

        int toolindex = 0; 
        foreach(ActivateButton button in _WheelButtons)
        {
            button.SetCorrespondingToolIndex(toolindex);
            toolindex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
