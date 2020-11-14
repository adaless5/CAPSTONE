using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickup : MonoBehaviour, ISaveable
{
    [SerializeField] int _CorrespondingEquipmentBeltIndex = 0;

   
    bool isUsed = false;

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
        
        if (isUsed) GetComponent<MeshRenderer>().enabled = false;
        else GetComponent<MeshRenderer>().enabled = true;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Belt belt = other.gameObject.GetComponentInChildren<Belt>();
            belt.ObtainEquipmentAtIndex(_CorrespondingEquipmentBeltIndex);
            isUsed = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isEnabled", isUsed);
    }   

    public void LoadDataOnSceneEnter()
    {
        isUsed = SaveSystem.LoadBool(gameObject.name, "isEnabled");
    }
}
