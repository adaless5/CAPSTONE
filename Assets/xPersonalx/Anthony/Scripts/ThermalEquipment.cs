using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalEquipment : Equipment
{
    ALTPlayerController _playerController; 
    bool bIsInThermalView = false;

    // Start is called before the first frame update
    public override void Start()
    {
        _playerController = FindObjectOfType<ALTPlayerController>(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        if (bIsActive && bIsObtained)
        {
            UseTool(); 
        }
    }

    public override void UseTool()
    {
        if (_playerController != null)
        {
            if(_playerController.CheckForUseEquipmentInput())
            {
                ThermalSkin[] ThermalObjs = FindObjectsOfType<ThermalSkin>();

                if (!bIsInThermalView)
                {
                    foreach (ThermalSkin obj in ThermalObjs)
                    {
                        obj.ChangeToThermalSkin();
                    }

                    bIsInThermalView = true;
                }
                else if (bIsInThermalView)
                {
                    foreach (ThermalSkin obj in ThermalObjs)
                    {
                        obj.ChangeToNormalSkin();
                    }

                    bIsInThermalView = false;
                }
            }
        }
    }
}
