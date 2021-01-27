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
        m_weaponClipSize = 1;
        m_startingOverstockAmmo = 12;
        
        m_reloadTime = 0.5f;
        _ammoController = FindObjectOfType<AmmoUI>().GetComponent<AmmoController>();
        _ammoController.InitializeAmmo(AmmoController.AmmoTypes.Explosive, m_weaponClipSize, m_startingOverstockAmmo);

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

        m_projectileforce *= m_scalars.ProjectileForce;
        m_fireRate = 5f * m_scalars.FireRate;
        m_projectileLifeTime *= m_scalars.FuzeTime;
        m_blastradius *= m_scalars.BlastRadius;
        m_blastforce *= m_scalars.ImpactForce;
        m_damageAmount = 50f * m_scalars.Damage;
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
        if (bIsReloading)
        {
            return;
        }
        if (_ammoController.NeedsReload())
        {
            StartCoroutine(OnReload());
            return;
        }

        if (m_playerController.CheckForUseWeaponInput())
        {
            if (m_bCanThrow && _ammoController.CanUseAmmo())
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
        _ammoController.UseAmmo();
    }

    IEnumerator OnReload()
    {
        bIsReloading = true;    
        yield return new WaitForSeconds(m_reloadTime);      
        _ammoController.Reload();
        bIsReloading = false;      
    }

    public override void AddUpgrade(WeaponScalars scalars)
    {
        m_scalars += scalars;
        m_projectileforce *= m_scalars.ProjectileForce;
        m_fireRate *= m_scalars.FireRate;
        m_projectileLifeTime *= m_scalars.FuzeTime;
        m_blastradius *= m_scalars.BlastRadius;
        m_blastforce *= m_scalars.ImpactForce;
        m_damageAmount *= m_scalars.Damage;

    }


    public override void SetHasAction(bool hasaction)
    {
        m_bHasActionUpgrade = hasaction;
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
    }
}
