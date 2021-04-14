﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour, ISaveable
{
    public bool bIsActive = false;
    public bool bIsObtained = false;

    public bool bIsWeapon = false;
    public virtual void Start()
    {
        LoadDataOnSceneEnter();
    }

    public abstract void Update();
    public abstract void UseTool();

    public virtual void Activate()
    {
        bIsActive = true;
        if(bIsWeapon)
             EventBroker.CallOnWeaponSwapIn();
    }

    public virtual void Deactivate()
    {
        bIsActive = false;
        if (bIsWeapon)
            EventBroker.CallOnWeaponSwapOut();
    }

    public void ObtainEquipment()
    {
        bIsObtained = true;
        SaveSystem.Save(gameObject.name, "bIsObtained", "Equipment", bIsObtained, SaveSystem.SaveType.EQUIPMENT);
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
    }
}
