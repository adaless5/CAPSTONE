using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButton : MonoBehaviour
{
    public Button _button;
    public Belt _Belt;
    public int _CorrespondingToolIndex;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsToolObtained())
        {
            _button.interactable = true;
        }
    }

    bool IsToolObtained()
    {
        return _Belt._Belt[_CorrespondingToolIndex].GetComponentInChildren<Tool>().bIsObtained;
    }
}
