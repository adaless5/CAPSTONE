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
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        _Belt = GetComponentInParent<Belt>();
        canvas = GetComponentInParent<Canvas>();
        _button = GetComponent<Button>();
        //_button.onClick.AddListener(EquipToolAtCorrespondingToolIndex); 

        _button.interactable = false;

        if (IsToolActive())
            _button.Select();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsToolObtained() && !IsToolActive())
        {
            _button.interactable = true;
        }
        else if (IsToolActive() && canvas.enabled)
        {
            _button.Select();
        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_Belt._items[_CorrespondingToolIndex].bIsActive)
        {
            EquipToolAtCorrespondingToolIndex();
            Debug.Log("Pointer Enter");

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
