using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CreatureWeapon : Weapon
{
    public Camera _camera;
    public ParticleSystem _spreadEffect;
    public Animator reticuleAnimator;
    GameObject _creatureProjectile;
    //public GG_Animations _GGAnimator;
    public Weapon_Animations _GGAnimator;
    bool _bcoroutineOutIsRunning = false;
    bool _bcoroutineInIsRunning = false;   

    //vvv EP model stuff
    public GameObject _gunObject;
    public GameObject _firingOrganObject;
    public GameObject _internalOrganObject;
    Vector3 _firingOrganSize;
    Vector3 _internalOrganSize;
    public float _pulseScale;
    public float _numberOfGlobsShot;
    public float _globuleFireSpeed;
    public float _fireAngleRange;
    public Vector2 _globSizeRange;
    float _pulseTime;
    Vector3 _currentScale;
    public GameObject _muzzlePointObject;
    //^^^ EP model stuff
    private void Awake()
    {
        base.Awake();
        LoadDataOnSceneEnter();
        _creatureProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Creature Projectile");
        m_weaponClipSize = 8;
        m_reloadTime = 3.0f;

        _firingOrganSize = _firingOrganObject.transform.localScale;
        _internalOrganSize = _internalOrganObject.transform.localScale;

        EventBroker.OnPlayerSpawned += InitWeaponControls;
        EventBroker.OnWeaponSwap += WeaponSwapOut;
        EventBroker.OnWeaponSwapIn += GGWeaponSwapIn;

        _GGAnimator = GetComponent<GG_Animations>();

    }

    void OrganPulse()
    {
        _pulseTime += Time.deltaTime * 3;
        _currentScale = new Vector3(Mathf.Sin(_pulseTime) * 0.1f, Mathf.Cos(_pulseTime) * 0.1f, Mathf.Cos(_pulseTime) * 0.1f);
        _firingOrganObject.transform.localScale = _firingOrganSize - _currentScale;
        _internalOrganObject.transform.localScale = _internalOrganSize + new Vector3(0, _currentScale.y, 0);
        if (_pulseTime > 360)
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
        base.Start();
        //Initializing Ammo Controller
        _ammoController = FindObjectOfType<AmmoUI>().GetComponent<AmmoController>();
        _ammoController.InitializeAmmo(AmmoController.AmmoTypes.Creature, m_weaponClipSize, m_weaponClipSize, m_ammoCapAmount);

        _camera = FindObjectOfType<Camera>();

        m_fireRate = 0.5f * m_upgradestats.FireRate;
        m_hitImpact = 10.0f * m_upgradestats.ImpactForce;
        m_damageAmount = 5.0f * m_upgradestats.Damage;
        m_maxDamageTime = 1f * m_upgradestats.DamageTime;
        m_projectileLifeTime = 6.0f * m_upgradestats.FuzeTime;

        _gunObject.SetActive(false);
    }


    public override void UseTool()
    {
        if(bCanShoot)
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
    }

    //For animations, swapping out weapons
    public IEnumerator SwapOutLogic()
    {
        _bcoroutineOutIsRunning = true;
        Debug.Log("Started GG SwapOutCoroutine at timestamp: " + Time.time);
        //Waits for default gun swap out animation to play before setting inactive
        //yield return new WaitForSeconds(0.667f);
        yield return new WaitForSeconds(1.2f);
        if (!bIsActive)
        {
            _gunObject.SetActive(false);
            bCanShoot = false;
        }
        else
        {
            _gunObject.SetActive(true);
        }
        Debug.Log("Finished GG SwapOutCoroutine at timestamp: " + Time.time);
        _bcoroutineOutIsRunning = false;
    }
    public IEnumerator GGSwapInLogic()
    {
        _bcoroutineInIsRunning = true;
        Debug.Log("Started GG SwapInCoroutine at timestamp: " + Time.time);
        //Waits for other weapon to finish swapping out before swapping in, currently only default gun at 1.133 seconds
        yield return new WaitForSeconds(1.2f);
        if (bIsActive)
        {
            _gunObject.SetActive(true);
            bCanShoot = true;
        }
        else
        {
            _gunObject.SetActive(false);
        }
        Debug.Log("Finished GG SwapInCoroutine at timestamp: " + Time.time);
        _bcoroutineInIsRunning = false;
    }
    public void WeaponSwapOut()
    {
        if (!_bcoroutineOutIsRunning && !bIsActive)
            StartCoroutine(SwapOutLogic());
    }

    public void GGWeaponSwapIn()
    {
        if (!_bcoroutineInIsRunning && bIsActive)
            StartCoroutine(GGSwapInLogic());
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
                //_gunObject.SetActive(true);
                //_firingOrganObject.SetActive(true);
                //_internalOrganObject.SetActive(true);
                UseTool();
                OnTarget();
                OrganPulse();
            }
            //else if (!bIsActive)
            //{
            //    _gunObject.SetActive(false);
            //    _firingOrganObject.SetActive(false);
            //    _internalOrganObject.SetActive(false);
            //}
        }
    }

    void OnShoot()
    {
        _GGAnimator._weaponAnimator.SetTrigger("HasFired");
        //Play shot
        AudioManager_CreatureWeapon amc = GetComponent<AudioManager_CreatureWeapon>();
        amc.TriggerShootCreatureWeapon();

        for (int i = 0; i < _numberOfGlobsShot; i++)
        {
            //Vector3 bulletDeviation = UnityEngine.Random.insideUnitCircle * 300.0f;
            //Quaternion rot = Quaternion.LookRotation(Vector3.forward * 200.0f + bulletDeviation);
            //Vector3 finalFowardVector = transform.rotation * rot * Vector3.forward;
            //finalFowardVector += transform.position;

            //GameObject creatureProjectile = Instantiate(_creatureProjectile, finalFowardVector, Quaternion.identity);
            //if (ObjectPool.Instance != null)
            //{
            //    GameObject creatureProjectile = ObjectPool.Instance.SpawnFromPool("Creature", _creatureProjectile, finalFowardVector, Quaternion.identity);
            //    float randomfloat = UnityEngine.Random.Range(0.1f, 0.5f);
            //    Vector3 randomSize = new Vector3(randomfloat, randomfloat, randomfloat);
            //    creatureProjectile.transform.localScale = randomSize;
            //    creatureProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * m_hitImpact, ForceMode.Impulse);
            //    creatureProjectile.GetComponent<CreatureProjectile>().InitCreatureProjectile(m_maxDamageTime, m_projectileLifeTime, m_damageAmount, m_bHasActionUpgrade);
            //    creatureProjectile.GetComponent<CreatureProjectile>().LinkAudioManager(amc);

            //}

            //vvv Evan's adjustments
            if (ObjectPool.Instance != null)
            {

                GameObject creatureProjectile = ObjectPool.Instance.SpawnFromPool("Creature", _creatureProjectile, _muzzlePointObject.transform.position, _muzzlePointObject.transform.rotation);
                float randomfloat = UnityEngine.Random.Range(_globSizeRange.x, _globSizeRange.y);
                Vector3 randomSize = new Vector3(randomfloat, randomfloat, randomfloat);
                creatureProjectile.transform.localScale = randomSize;
                Vector3 randomRot = new Vector3(UnityEngine.Random.Range(-_fireAngleRange, _fireAngleRange), UnityEngine.Random.Range(-_fireAngleRange, _fireAngleRange), 0.0f);
                creatureProjectile.transform.Rotate(randomRot);
                creatureProjectile.GetComponent<Rigidbody>().AddForce(creatureProjectile.transform.forward * _globuleFireSpeed, ForceMode.Impulse);
                creatureProjectile.GetComponent<CreatureProjectile>().InitCreatureProjectile(m_maxDamageTime, m_projectileLifeTime, m_damageAmount, m_bHasActionUpgrade);
                creatureProjectile.GetComponent<CreatureProjectile>().LinkAudioManager(amc);

            }
            //^^^ Evan's adjustments

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
        _GGAnimator.SetReloadAnimationSpeed(m_reloadTime);

        if (bIsActive)
        {
            //Play Reload Sound
            GetComponent<AudioManager_CreatureWeapon>().TriggerReloadCreatureWeapon();
        }


        bIsReloading = true;
        // gunAnimator.SetBool("bIsReloading", true);
        _GGAnimator.TriggerReloadAnimation();
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
        _GGAnimator.SetReloadAnimationSpeed(m_reloadTime);
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
