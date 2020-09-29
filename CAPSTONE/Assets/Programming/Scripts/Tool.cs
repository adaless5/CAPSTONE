using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public bool bIsActive = false;
    public bool bIsObtained = false;

    public virtual void Start()
    {
        bIsActive = false;
        bIsObtained = false;
    }

    public abstract void Update();
    public abstract void UseTool();

    public virtual void Activate()
    {
        bIsActive = true;
    }

    public virtual void Deactivate()
    {
        bIsActive = false;
    }

    public void ObtainEquipment()
    {
        bIsObtained = true;
    }
}
