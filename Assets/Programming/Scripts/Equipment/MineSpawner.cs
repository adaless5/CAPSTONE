using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : Weapon
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
    public GameObject grenadeInHand;
    public ALTPlayerController m_playerController;
    Weapon_Animations _grenadeAnimator;
    bool _bcoroutineOutIsRunning = false;
    bool _bcoroutineInIsRunning = false;
    public GameObject _grenadeArmModel;
    public Transform _spawnLocation;

    bool m_bCanThrow = true;
    float m_timer;
    public override void Start()
    {
        base.Start();
        m_weaponClipSize = 1;
        m_startingOverstockAmmo = 12;
        m_reloadTime = 0.5f;
        _ammoController = FindObjectOfType<AmmoUI>().GetComponent<AmmoController>();
        _ammoController.InitializeAmmo(AmmoController.AmmoTypes.Explosive, m_weaponClipSize, m_startingOverstockAmmo, m_ammoCapAmount);
        //_grenadeArmModel = transform.GetChild(2).gameObject;
        _grenadeArmModel = gameObject.transform.Find("Grenade_Model").gameObject;
        _spawnLocation = gameObject.transform.Find("SpawnLocation");
        _grenadeArmModel.SetActive(false);
        //_grenadeArmModel.SetActive(false);

    }

    void Awake()
    {
        base.Awake();

        if (m_playerController == null)
        {
            m_playerController = FindObjectOfType<ALTPlayerController>();
        }

        m_projectileforce *= m_upgradestats.ProjectileForce;
        m_fireRate = 2f * m_upgradestats.FireRate;
        m_projectileLifeTime *= m_upgradestats.FuzeTime;
        m_blastradius *= m_upgradestats.BlastRadius;
        m_blastforce *= m_upgradestats.ImpactForce;
        m_damageAmount = 50f * m_upgradestats.Damage;
        _grenadeAnimator = GetComponent<Grenade_Animations>();
        EventBroker.OnWeaponSwap += WeaponSwapOut;
        EventBroker.OnWeaponSwapIn += GrenadeWeaponSwapIn;
        m_timer = m_fireRate;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (bIsActive)
        {
            if (m_playerController.m_ControllerState == ALTPlayerController.ControllerState.Play)
                UseTool();
            if (m_bCanThrow)
            {
                grenadeInHand.SetActive(true);
            }
        }
        else
        {
            grenadeInHand.SetActive(false);
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
                m_bCanThrow = false;
                m_timer = m_fireRate;
                ThrowMine();
                grenadeInHand.SetActive(false);
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
                grenadeInHand.SetActive(true);
                //Debug.Log("RESET");
            }
        }
    }

    void ThrowMine()
    {
        if (bCanShoot)
        {
            StartCoroutine(SpawnMine());
            _grenadeAnimator._weaponAnimator.SetTrigger("Fired");

            _ammoController.UseAmmo();
        }
    }

    IEnumerator SpawnMine()
    {
        yield return new WaitForSeconds(0.45f);
        GameObject mine = Instantiate(minePrefab, _spawnLocation.position, _spawnLocation.rotation);
        //Debug.Log("MINE SPAWNED");
        if (mine)
        {
            mine.GetComponent<Rigidbody>().AddForce(_spawnLocation.forward * m_projectileforce);
            mine.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), -90f));
            mine.GetComponent<Mine>().InitMine(m_projectileLifeTime, m_blastradius, m_blastforce, m_damageAmount, m_bHasActionUpgrade);

            //Trigger Grenade Sounds
            AudioManager_Grenade audioManager = GetComponent<AudioManager_Grenade>();
            mine.SendMessage("BindAudioManager", audioManager);
            audioManager.TriggerThrowGrenade();
            StartCoroutine(StartExplosionSounds(audioManager));
            //
        }
    }

    IEnumerator OnReload()
    {
        _grenadeAnimator.SetReloadAnimationSpeed(m_reloadTime);
        bIsReloading = true;
        yield return new WaitForSeconds(m_reloadTime);
        _ammoController.Reload();
        bIsReloading = false;
        _grenadeAnimator.TriggerReloadAnimation();
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


    //public override void SetHasAction(bool hasaction)
    //{
    //    m_bHasActionUpgrade = hasaction;
    //}

    IEnumerator StartExplosionSounds(AudioManager_Grenade amg)
    {
        yield return new WaitForSeconds(.8f);
        amg.TriggerGrenadeCook();
        yield return new WaitForSeconds(2f);
        amg.TriggerGrenadeExplode();
    }

    public IEnumerator SwapOutLogic()
    {
        _bcoroutineOutIsRunning = true;        
        yield return new WaitForSeconds(1.2f);
        if (!bIsActive)
        {
            _grenadeArmModel.SetActive(false);
            bCanShoot = false;
        }
        else
        {
            _grenadeArmModel.SetActive(true);
        }        
        _bcoroutineOutIsRunning = false;
    }

    public IEnumerator SwapInLogic()
    {
        _bcoroutineInIsRunning = true;        

        yield return new WaitForSeconds(1.2f);
        if (bIsActive)
        {
            _grenadeArmModel.SetActive(true);
            bCanShoot = true;
        }
        else
        {
            _grenadeArmModel.SetActive(false);
        }        
        _bcoroutineInIsRunning = false;
    }

    public void WeaponSwapOut()
    {
        if (!_bcoroutineOutIsRunning && !bIsActive)
            StartCoroutine(SwapOutLogic());
    }

    public void GrenadeWeaponSwapIn()
    {
        if (!_bcoroutineInIsRunning && bIsActive)
            StartCoroutine(SwapInLogic());
    }

}
