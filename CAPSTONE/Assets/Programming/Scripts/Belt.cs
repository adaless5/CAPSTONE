using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Belt : MonoBehaviour
{
    public List<GameObject> _Belt;
    public int selectedWeaponIndex = 0;

    void Start()
    {

    }

    void Update()
    {
        //WeaponScrollWheel();
    }

    void ToolScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") * 10 < 0f)
        {
            selectedWeaponIndex--;

            if (selectedWeaponIndex < 0)
            {
                selectedWeaponIndex = _Belt.Count - 1;
            }

            ChangeActiveEquipment();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") * 10 > 0f)
        {
            selectedWeaponIndex++;

            if (selectedWeaponIndex > _Belt.Count - 1)
            {
                selectedWeaponIndex = 0;
            }

            ChangeActiveEquipment();
        }
    }

    void ChangeActiveEquipment()
    {
        int i = 0;

        foreach (GameObject equipment in _Belt)
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

    public void ObtainEquipmentAtIndex(int index)
    {
        _Belt[index].GetComponentInChildren<Tool>().ObtainEquipment();

        selectedWeaponIndex = index;

        ChangeActiveEquipment();
    }
}
