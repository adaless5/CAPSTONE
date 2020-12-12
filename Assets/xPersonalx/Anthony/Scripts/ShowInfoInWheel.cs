using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfoInWheel : MonoBehaviour
{
    Image[] _Icon;
    Text _Text;
    Belt _Belt;
    public int _CorrespondingToolIndex = 0;

    private bool bCorrespondingToolIsObtained;

    // Start is called before the first frame update
    void Start()
    {
        _Icon = gameObject.GetComponentsInChildren<Image>();
        _Text = gameObject.GetComponentInChildren<Text>();

        _Icon[1].enabled = false;
        _Text.enabled = false;

        _Belt = GetComponentInParent<Belt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsToolObtained())
        {
            _Icon[1].enabled = true;
            _Text.enabled = true;
        }
    }

    bool IsToolObtained()
    {
        try
        {
            return _Belt._items[_CorrespondingToolIndex].GetComponentInChildren<Tool>().bIsObtained;
        }
        catch
        {
            return false;
        }
    }
}
