using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] int Damage = 50;
    public ALTPlayerController playerController;

    Animator animation;
    BoxCollider hitbox;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider>();

        hitbox.enabled = false;
        hitbox.isTrigger = true;
        GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.CheckForAttackPushed())
        {
            animation.SetBool("attacking", true);
            hitbox.enabled = true;
        }
        else if(playerController.CheckForAttackReleased())
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
