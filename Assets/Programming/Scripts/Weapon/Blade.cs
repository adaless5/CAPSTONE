using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : Equipment, ISaveable
{
    [SerializeField] int Damage { get; } = 50;
    public ALTPlayerController playerController;

    Animator _animationswing;
    BoxCollider _hitbox;
    bool _bisAttacking;

    Camera _cam;

    // Start is called before the first frame update
    public override void Start()
    {
        //base.Start(); 
        _bisAttacking = false;
        _animationswing = GetComponent<Animator>();
        _hitbox = GetComponent<BoxCollider>();

        _hitbox.enabled = false;
        _hitbox.isTrigger = true;
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
        if (playerController.CheckForUseEquipmentInput() && _bisAttacking == false)
        {
            Debug.Log("Swing");
            _animationswing.SetTrigger("Swing");
            _hitbox.enabled = true;
            _bisAttacking = true;
        }
        else if (playerController.CheckForUseEquipmentInputReleased() && _bisAttacking == true)
        {
            _hitbox.enabled = false;
            _bisAttacking = false;
        }

    }

    public int GetDamage()
    {
        return Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        DestructibleObject obj = other.GetComponentInParent<DestructibleObject>();
        if (obj)
        {
            Debug.Log("HIT");
            obj.Break(gameObject.tag);
        }
        _hitbox.enabled = false;
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
