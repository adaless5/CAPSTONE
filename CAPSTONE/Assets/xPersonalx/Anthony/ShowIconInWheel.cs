using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowIconInWheel : MonoBehaviour
{
    public Image _Icon;
    public Belt _Belt;
    public int _CorrespondingToolIndex;

    private bool bCorrespondingToolIsObtained;

    // Start is called before the first frame update
    void Start()
    {
        _Icon = GetComponent<Image>();
        _Icon.enabled = false;
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
        return _Belt._Belt[_CorrespondingToolIndex].GetComponentInChildren<Tool>().bIsObtained;
    }
}
