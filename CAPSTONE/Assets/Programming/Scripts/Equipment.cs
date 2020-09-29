using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Equipment : MonoBehaviour
{
    public bool bIsActive;
    public bool bObtainedEquipment;

    void Start()
    {

    }

    public virtual void Update()
    {

    }

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
        bObtainedEquipment = true;
    }

    public abstract void UseEquipment();
}
