using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCube : MonoBehaviour, ISaveable
{
    public Material material;
    public Material material2;

    bool colorToggle = false;

    int savethisthing = 0;

    void Awake()
    {
        //Load scene data and subscribe to Save Event.
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
    }
    public void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    void Update()
    {
        //update color
        if (!colorToggle) GetComponent<Renderer>().material = material;
        else GetComponent<Renderer>().material = material2;
    }

    public void ChangeColor()
    {
        if (!colorToggle)
        {
            GetComponent<Renderer>().material = material;
            colorToggle = true;
        }
        else
        {
            GetComponent<Renderer>().material = material2;
            colorToggle = false;
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "colorToggle", gameObject.scene.name, colorToggle);

    }

    public void LoadDataOnSceneEnter()
    {
        colorToggle = SaveSystem.LoadBool(gameObject.name, "colorToggle", gameObject.scene.name);
    }



}


