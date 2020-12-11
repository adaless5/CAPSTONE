using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActivateButton : MonoBehaviour, IPointerEnterHandler
{
    Button _button;
    Belt _Belt;
    public int _CorrespondingToolIndex;
    CanvasGroup _canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        _Belt = GetComponentInParent<Belt>();

        _button = GetComponent<Button>();
        //_button.onClick.AddListener(EquipToolAtCorrespondingToolIndex); 

        _button.interactable = false;

        if (IsToolActive())
            _button.Select();

        _canvasGroup = GetComponentInParent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_canvasGroup != null)
        {
            if (_canvasGroup.interactable == true)
            {
                if (IsToolObtained() && !IsToolActive())
                {
                    _button.interactable = true;
                }
                else if (IsToolActive())
                {
                    _button.Select();
                }
            }
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        EquipToolAtCorrespondingToolIndex();
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

    bool IsToolActive()
    {
        try
        {
            return _Belt._items[_CorrespondingToolIndex].GetComponentInChildren<Tool>().bIsActive;
        }
        catch { return false; }
    }

    void EquipToolAtCorrespondingToolIndex()
    {
        _Belt.EquipToolAtIndex(_CorrespondingToolIndex);
    }

    public void SetCorrespondingToolIndex(int index)
    {
        _CorrespondingToolIndex = index;
    }
}
