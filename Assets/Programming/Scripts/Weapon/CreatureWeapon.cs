using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CreatureWeapon : Weapon, ISaveable
{
    public Camera _camera;
    public ParticleSystem _spreadEffect;
    public Animator reticuleAnimator;
    GameObject _creatureProjectile;

    //vvv EP model stuff
    public GameObject _gunObject;
    public GameObject _firingOrganObject;
    public GameObject _internalOrganObject;
    Vector3 _firingOrganSize;
    Vector3 _internalOrganSize;
    public float _pulseScale;
    float _pulseTime;
    Vector3 _currentScale;
    //^^^ EP model stuff
    private void Awake()
    {
        base.Awake();
        _creatureProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Creature Projectile");
        m_weaponClipSize = 8;
        m_reloadTime = 3.0f;
        _firingOrganSize = _firingOrganObject.transform.localScale;
        _internalOrganSize = _internalOrganObject.transform.localScale;

        EventBroker.OnPlayerSpawned += InitWeaponControls;
    }

    void OrganPulse()
    {
        _pulseTime += Time.deltaTime * 3;
        _currentScale =  new Vector3( Mathf.Sin(_pulseTime)*0.1f, Mathf.Cos(_pulseTime) * 0.1f, Mathf.Cos(_pulseTime) * 0.1f);
        _firingOrganObject.transform.localScale = _firingOrganSize - _currentScale;
        _internalOrganObject.transform.localScale = _internalOrganSize + new Vector3 (0, _currentScale.y,0 );
        if(_pulseTime>360)
        {
            _pulseTime = 0.0f;
        }
    }
    public void InitWeaponControls(GameObject player)
    {
        _playerController._controls.Player.Reload.performed += ctx => Reload();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        //Initializing Ammo Controller
        _ammoController = FindObjectOfType<AmmoUI>().GetComponent<AmmoController>();
        _ammoController.InitializeAmmo(AmmoController.AmmoTypes.Creature, m_weaponClipSize, m_weaponClipSize, m_ammoCapAmount);

        _camera = FindObjectOfType<Camera>();
        bIsActive = false;
        bIsObtained = false;

        m_fireRate = 0.5f * m_upgradestats.FireRate;
        m_hitImpact = 10.0f * m_upgradestats.ImpactForce;
        m_damageAmount = 5.0f * m_upgradestats.Damage;
        m_maxDamageTime = 1f * m_upgradestats.DamageTime;
        m_projectileLifeTime = 6.0f * m_upgradestats.FuzeTime;
    }


    public override void UseTool()
    {
        if (bIsReloading)
        {
            return;
        }

        //Reloads automatically at 0 or if player users reload input "R"       
        if (_ammoController.NeedsReload())
        {
            StartCoroutine(OnReload());
            return;
        }

        if (_playerController.CheckForUseWeaponInput() && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            if (_ammoController.CanUseAmmo())
            {
                OnShoot();
            }
        }
    }

    private void Reload()
    {
        if (_ammoController.CanReload())
        {
            StartCoroutine(OnReload());
        }
    }

    public override void Update()
    {
        if (_playerController != null)
        {
            if (bIsActive && _playerController.m_ControllerState == ALTPlayerController.ControllerState.Play)
            {
                _gunObject.SetActive(true);
                _firingOrganObject.SetActive(true);
                _internalOrganObject.SetActive(true);
                UseTool();
                OnTarget();
                OrganPulse();
            }
            else if (!bIsActive)
            {
                _gunObject.SetActive(false);
                _firingOrganObject.SetActive(false);
                _internalOrganObject.SetActive(false);
            }
        }
    }

    void OnShoot()
    {
        //Play shot
        AudioManager_CreatureWeapon amc = GetComponent<AudioManager_CreatureWeapon>();
        amc.TriggerShootCreatureWeapon();

        for (int i = 0; i < m_weaponClipSize; i++)
        {
            Vector3 bulletDeviation = UnityEngine.Random.insideUnitCircle * 300.0f;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * 200.0f + bulletDeviation);
            Vector3 finalFowardVector = transform.rotation * rot * Vector3.forward;
            finalFowardVector += transform.position;

            //GameObject creatureProjectile = Instantiate(_creatureProjectile, finalFowardVector, Quaternion.identity);
            if (ObjectPool.Instance != null)
            {

                GameObject creatureProjectile = ObjectPool.Instance.SpawnFromPool("Creature", _creatureProjectile, finalFowardVector, Quaternion.identity);
                float randomfloat = UnityEngine.Random.Range(0.1f, 0.5f);
                Vector3 randomSize = new Vector3(randomfloat, randomfloat, randomfloat);
                creatureProjectile.transform.localScale = randomSize;
                creatureProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * m_hitImpact, ForceMode.Impulse);
                creatureProjectile.GetComponent<CreatureProjectile>().InitCreatureProjectile(m_maxDamageTime, m_projectileLifeTime, m_damageAmount, m_bHasActionUpgrade);
                creatureProjectile.GetComponent<CreatureProjectile>().LinkAudioManager(amc);

            }
            else
            {
                Debug.LogError("Object Pool not initialized! Create an Object Pool prefab");
            }
        }

        //Using ammo      
        _ammoController.UseAmmo();
    }

    IEnumerator OnReload()
    {
        if (bIsActive)
        {
            //Play Reload Sound
            GetComponent<AudioManager_CreatureWeapon>().TriggerReloadCreatureWeapon();
        }


        bIsReloading = true;
        // gunAnimator.SetBool("bIsReloading", true);

        yield return new WaitForSeconds(m_reloadTime);
        _ammoController.Reload();
        bIsReloading = false;

        //Play reload animations once set up         
        //gunAnimator.SetBool("bIsReloading", false);
    }

    public override void AddUpgrade(WeaponUpgrade upgrade)
    {
        m_upgradestats += upgrade;
        m_damageAmount *= upgrade.Damage + 1;
        m_fireRate *= upgrade.FireRate + 1;
        m_hitImpact *= upgrade.ImpactForce + 1;
        m_projectileLifeTime *= upgrade.FuzeTime + 1;
        m_maxDamageTime *= upgrade.DamageTime + 1;
        if (upgrade.HasAction) m_bHasActionUpgrade = true;

        m_currentupgrades.Add(upgrade.Type);
    }

    private void OnTarget()
    {
        RaycastHit targetInfo;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out targetInfo, m_weaponRange))
        {
            Health target = targetInfo.transform.GetComponent<Health>();
            if (target != null && target.gameObject.tag != "Player")
            {
                reticuleAnimator.SetBool("isTargetted", true);
            }
            else
            {
                reticuleAnimator.SetBool("isTargetted", false);
            }
        }
    }

    public void LoadDataOnSceneEnter()
    {

    }
}
