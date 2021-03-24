using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : Equipment, ISaveable
{
    [SerializeField] int Damage { get; } = 25;
    [SerializeField] float KnockBackForce = 1500f;
    public ALTPlayerController playerController;

    Animator _animationswing;
    BoxCollider _hitbox;
    bool _bisAttacking;
    GameObject prevHit;

    // Start is called before the first frame update
    public override void Start()
    {
        //base.Start(); 

        if(playerController == null)
        {
            playerController = FindObjectOfType<ALTPlayerController>();
        }

        _bisAttacking = false;
        _animationswing = GetComponent<Animator>();
        _hitbox = GetComponent<BoxCollider>();

        _hitbox.enabled = false;
        _hitbox.isTrigger = true;
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer obj in meshs)
        {
            obj.enabled = false;
        }


    }

    void Awake()
    {
        LoadDataOnSceneEnter();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (bIsActive && bIsObtained)
        {
            MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer obj in meshs)
            {
                obj.enabled = true;
            }
               
            UseTool();
        }
        else if (!bIsActive)
        {
            MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer obj in meshs)
            {
                obj.enabled = false;
            }
        }
    }

    public override void UseTool()
    {
        if(playerController.CheckForUseEquipmentInput() && _animationswing.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animationswing.SetBool("attacking", true);
        }
        if(_animationswing.GetCurrentAnimatorStateInfo(0).IsName("BladeAttacking"))
        {
            _animationswing.SetBool("attacking", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (prevHit != other.gameObject && other.tag != "Player")
        {
            prevHit = other.gameObject;
            if (other.GetComponentInParent<DestructibleObject>())
            {
                DestructibleObject obj = other.GetComponentInParent<DestructibleObject>();
                if (obj)
                {
                    obj.Break(gameObject.tag);
                    return;
                }
            }
            ///EP ItemContainer call vvv
            if (other.GetComponentInParent<ItemContainer>())
            {
                ItemContainer obj = other.GetComponentInParent<ItemContainer>();
                if (obj)
                {
                    obj.Break(gameObject.tag);
                    return;
                }
            }
        }
        ///EP ItemContainer call ^^^
        ///EP Eyelight call vvv
        if (other.GetComponentInParent<EyeLight>())
        {
            EyeLight obj = other.GetComponentInParent<EyeLight>();
            if (obj)
            {
                obj.Hit();
                return;
            }
        }
        ///EP EYelight call ^^^
        if (other.GetComponentInParent<Health>())
        {
            Health target = other.transform.GetComponent<Health>();
            if (target != null)
            {
                target.TakeDamage(Damage);
            }

            if (other.GetComponent<Rigidbody>())
            {
                Rigidbody bodytarget = other.transform.GetComponent<Rigidbody>();

                if (bodytarget != null)
                {
                    Vector3 hitDir = playerController.transform.position - bodytarget.transform.position;
                    bodytarget.AddForce(hitDir.normalized * -KnockBackForce);
                }
            }
        }
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
    }
}
