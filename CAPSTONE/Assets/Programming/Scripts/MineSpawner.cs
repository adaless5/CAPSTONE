using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : Weapon
{
    public float Force = 20f;
    public float CoolDown = 5f;
    public GameObject minePrefab;
    public ALTPlayerController m_playerController;

    bool m_bCanThrow = true;
    float timer;
    public override void Start()
    {
        timer = CoolDown;
        bIsActive = false;
        bIsObtained = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(bIsActive && m_playerController.m_ControllerState == ALTPlayerController.ControllerState.Play)
        {
            UseTool();
        }
    }

    public override void UseTool()
    {
        if (m_playerController.CheckForUseWeaponInput())
        {
            if (m_bCanThrow)
            {
                ThrowMine();
                m_bCanThrow = false;
                timer = CoolDown;
                Debug.Log("THROWN");
            }
        }


        if (!m_bCanThrow)
        {
            timer -= Time.deltaTime;
            Debug.Log("COOLDOWN");

            if (timer <= 0f)
            {
                m_bCanThrow = true;
                Debug.Log("RESET");
            }
        }
    }

    void ThrowMine()
    {
        GameObject mine = Instantiate(minePrefab, transform.position, transform.rotation);
        if(mine)
        {
            mine.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
        }
    }
}
