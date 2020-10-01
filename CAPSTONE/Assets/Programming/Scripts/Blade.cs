using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : Equipment
{
    [SerializeField] int Damage = 50;
    public ALTPlayerController playerController;

    Animator animation;
    BoxCollider hitbox;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); 
        animation = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider>();

        hitbox.enabled = false;
        hitbox.isTrigger = true;
        GetComponent<MeshRenderer>().enabled = false;
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
            animation.SetBool("attacking", true);
            hitbox.enabled = true;
        }
        else if (playerController.CheckForUseEquipmentInputReleased())
        {
            animation.SetBool("attacking", false);
            hitbox.enabled = false;
        }
    }

    public int GetDamage()
    {
        return Damage;
    }
}
