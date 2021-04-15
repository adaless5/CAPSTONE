using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public abstract class Tool : MonoBehaviour, ISaveable
{
    public bool bIsActive = false;
    public bool bIsObtained = false;


    public virtual void Start()
    {
        SceneManager.sceneLoaded += UpdateToolData;
    }

    public abstract void Update();
    public abstract void UseTool();

    public virtual void Activate()
    {
        bIsActive = true;
        //EventBroker.CallOnWeaponSwapIn();
    }

    public virtual void Deactivate()
    {
        bIsActive = false;
        //EventBroker.CallOnWeaponSwapOut();
    }

    public void ObtainEquipment()
    {
        EventBroker.CallOnAutoSave();
        bIsObtained = true;
        SaveSystem.Save(gameObject.name, "bIsObtained", "Equipment", bIsObtained, SaveSystem.SaveType.EQUIPMENT);
    }

    public void UpdateToolData(Scene scene, LoadSceneMode scenemode)
    {
        if (scene.name == "Loading_Scene")
            LoadDataOnSceneEnter();
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
        bIsActive = SaveSystem.LoadBool(gameObject.name, "bIsActive", "Equipment");

        if (!bIsActive)
            Deactivate();

    }
}
