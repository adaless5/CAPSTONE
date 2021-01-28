using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : Weapon, ISaveable
{
    //[Header("Spawner Settings")]
    //public float Force = 800f;
    //public float CoolDown = 5f;

    //[Header("Mine Settings")]
    //public float FuzeTimer = 3f;
    //public float BlastRadius = 10f;
    //public float ExplosionForce = 2000f;
    //public float Damage = 50;

    public GameObject minePrefab;
    public ALTPlayerController m_playerController;

    bool m_bCanThrow = true;
    float m_timer;
    public override void Start()
    {
        m_timer = m_fireRate;
        bIsActive = false;
        bIsObtained = false;
    }

    void Awake()
    {
        base.Awake();
        LoadDataOnSceneEnter();

        if(m_playerController == null)
        {
            m_playerController = FindObjectOfType<ALTPlayerController>();
        }

        m_projectileforce *= m_upgradestats.ProjectileForce;
        m_fireRate = 5f * m_upgradestats.FireRate;
        m_projectileLifeTime *= m_upgradestats.FuzeTime;
        m_blastradius *= m_upgradestats.BlastRadius;
        m_blastforce *= m_upgradestats.ImpactForce;
        m_damageAmount = 50f * m_upgradestats.Damage;
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
                m_timer = m_fireRate;
                //Debug.Log("THROWN");
            }
        }


        if (!m_bCanThrow)
        {
            m_timer -= Time.deltaTime;
            //Debug.Log("COOLDOWN");

            if (m_timer <= 0f)
            {
                m_bCanThrow = true;
                //Debug.Log("RESET");
            }
        }
    }

    void ThrowMine()
    {
        GameObject mine = Instantiate(minePrefab, transform.position, transform.rotation);
        if(mine)
        {
           // mine.GetComponent<Mine>().InitMine(
            mine.GetComponent<Rigidbody>().AddForce(transform.forward * m_projectileforce);
            mine.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), -90f));
            mine.GetComponent<Mine>().InitMine(m_projectileLifeTime, m_blastradius, m_blastforce, m_damageAmount, m_bHasActionUpgrade);
        }
    }

    public override void AddUpgrade(WeaponUpgrade upgrade)
    {
        m_upgradestats += upgrade;
        m_projectileforce *= upgrade.ProjectileForce + 1;
        m_fireRate *= upgrade.FireRate + 1;
        m_projectileLifeTime *= upgrade.FuzeTime + 1;
        m_blastradius *= upgrade.BlastRadius + 1;
        m_blastforce *= upgrade.ImpactForce + 1;
        m_damageAmount *= upgrade.Damage + 1;
        if (upgrade.HasAction) m_bHasActionUpgrade = true;

        m_currentupgrades.Add(upgrade.Type);
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
    }
}
