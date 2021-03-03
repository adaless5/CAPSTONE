using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    public Tool[] _items;
    public int selectedWeaponIndex = 0;

    protected void Start()
    {
        _items = GetComponentsInChildren<Tool>();

    }

    void ToolScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") * 10 < 0f)
        {
            selectedWeaponIndex--;

            if (selectedWeaponIndex < 0)
            {
                selectedWeaponIndex = _items.Length - 1;
            }

            ChangeActiveEquipment();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") * 10 > 0f)
        {
            selectedWeaponIndex++;

            if (selectedWeaponIndex > _items.Length - 1)
            {
                selectedWeaponIndex = 0;
            }

            ChangeActiveEquipment();
        }
    }

    public void ChangeActiveEquipment()
    {
        int i = 0;

        foreach (Tool equipment in _items)
        {
            if (i == selectedWeaponIndex)
            {
                equipment.GetComponentInChildren<Tool>().Activate();
            }
            else if (i != selectedWeaponIndex)
            {
                equipment.GetComponentInChildren<Tool>().Deactivate();
            }

            i++;
        }
    }

    public void EquipToolAtIndex(int index)
    {
        if (_items[index].GetComponentInChildren<Tool>().bIsObtained)
        {
            selectedWeaponIndex = index;
            ChangeActiveEquipment();
        }
    }

    public void ObtainEquipmentAtIndex(int index)
    {
        try
        {
            _items[index].GetComponentInChildren<Tool>().ObtainEquipment();

        }
        catch
        {

        }
    }

    public Tool GetToolAtIndex(int index)
    {
        return _items[index].GetComponentInChildren<Tool>();
    }
}
