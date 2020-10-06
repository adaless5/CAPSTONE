using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButton : MonoBehaviour
{
    public Button _button;
    Belt _Belt;
    public int _CorrespondingToolIndex;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = false;

        _Belt = GetComponentInParent<Belt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsToolObtained() && !IsToolActive())
        {
            _button.interactable = true;
        }
        else if (IsToolActive())
        {
            _button.interactable = false;
        }
    }

    bool IsToolObtained()
    {
        try{
            return _Belt._items[_CorrespondingToolIndex].GetComponentInChildren<Tool>().bIsObtained;
        }
        catch{
            return false;
        }
    }

    bool IsToolActive()
    {
        try
        {
            return _Belt._items[_CorrespondingToolIndex].GetComponentInChildren<Tool>().bIsActive;
        }
        catch{return false;}
    }
}
