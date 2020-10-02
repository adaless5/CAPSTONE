using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowIconInWheel : MonoBehaviour
{
    public Image _Icon;
    Belt _Belt;
    public int _CorrespondingToolIndex = 0;

    private bool bCorrespondingToolIsObtained;

    // Start is called before the first frame update
    void Start()
    {
        _Icon = GetComponent<Image>();
        _Icon.enabled = false;

        _Belt = GetComponentInParent<Belt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsToolObtained())
        {
            _Icon.enabled = true;
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
