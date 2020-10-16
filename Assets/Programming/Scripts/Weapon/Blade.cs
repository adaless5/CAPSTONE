using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : Equipment, ISaveable
{
    [SerializeField] int Damage = 50;
    public ALTPlayerController playerController;

    Animator animationswing;
    BoxCollider hitbox;

    Camera _cam;

    // Start is called before the first frame update
    public override void Start()
    {
        //base.Start(); 

        animationswing = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider>();

        hitbox.enabled = false;
        hitbox.isTrigger = true;
        GetComponent<MeshRenderer>().enabled = false;

        
    }

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    // Update is called once per frame
    public override void Update()
    {
        

        if (bIsActive && bIsObtained)
        {
            GetComponent<MeshRenderer>().enabled = true;
            UseTool();
        }
        else if (!bIsActive)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void UseTool()
    {
        if (playerController.CheckForUseEquipmentInput())
        {
            animationswing.SetBool("attacking", true);
            hitbox.enabled = true;
        }
        else if (playerController.CheckForUseEquipmentInputReleased())
        {
            animationswing.SetBool("attacking", false);
            hitbox.enabled = false;
        }
    }

    public int GetDamage()
    {
        return Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.tag);
        DestructibleObject obj = other.GetComponentInParent<DestructibleObject>();
        if (obj)
        {
            Debug.Log("HIT");
            obj.Break(gameObject.tag);
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "bIsActive", bIsActive);
        SaveSystem.Save(gameObject.name, "bIsObtained", bIsObtained);

    }   

    public void LoadDataOnSceneEnter()
    {
        bIsActive = SaveSystem.LoadBool(gameObject.name, "bIsActive");
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained");
    }
}
