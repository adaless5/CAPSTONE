using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class WeaponBase : Weapon, ISaveable
{ 

    [Header("UI Elements - ParticleFX and Reticule")]
    public ParticleSystem muzzleFlash;
    public GameObject impactFX;
    public Animator reticuleAnimator;  
    [SerializeField]
    Animator gunAnimator;

    [Header("Camera Settings")]
    public Camera gunCamera;
    
    float m_timeElapsed;
    float m_lerpDuration = 3f;

    bool _bPlayedNewShellSound;

    void Awake()
    {
        base.Awake();        
        LoadDataOnSceneEnter();

        //TODO: Readd Save implementation
        //SaveSystem.SaveEvent += SaveDataOnSceneChange;

        m_weaponClipSize = 6 * (int)m_upgradestats.ClipSize;
        m_reloadTime = 2.0f * m_upgradestats.ReloadTime;
        m_fireRate = 1.5f * m_upgradestats.FireRate; //Default Gun shoots every 1.25 seconds, can be adjusted in editor - VR
        m_hitImpact = 50.0f * m_upgradestats.ImpactForce;
        m_weaponRange = 50.0f * m_upgradestats.Range;
        m_fireStart = 0.0f;       

        EventBroker.OnPlayerSpawned += InitWeaponControls;
        gunAnimator = GetComponent<Animator>();

        _bPlayedNewShellSound = false;
    }

    public void InitWeaponControls(GameObject player)
    {
        //Controls initializing for reloading && trying to shoot with no ammo

        _playerController._controls.Player.Reload.performed += ctx => Reload();
        _playerController._controls.Player.Shoot.started += ctx => TryShoot();
    }

    public override void Start()
    { 
        gunCamera = GameObject.FindObjectOfType<Camera>();

        _ammoController = FindObjectOfType<AmmoUI>().GetComponent<AmmoController>();
        _ammoController.InitializeAmmo(AmmoController.AmmoTypes.Default, m_weaponClipSize, m_weaponClipSize);
        GetComponent<MeshRenderer>().enabled = true;
        bIsActive = true;
        bIsObtained = true;

    }

    void OnEnable()
    {

        bIsReloading = false;       
    }


    void OnDisable()
    {
        //TODO: Readd save implementation
        //SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (_playerController != null)
        {
            if (bIsActive && _playerController.m_ControllerState == ALTPlayerController.ControllerState.Play)
            {
                GetComponent<MeshRenderer>().enabled = true;
                UseTool();
                OnTarget();                
            }
            else if (!bIsActive)
            {               
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public override void UseTool()
    {
        if (bIsReloading)
        {
            return;
        }

        //Reloads automatically at 0 or if player users reload input "R"        
        if(_ammoController.NeedsReload())
        {
            StartCoroutine(OnReload());
            return;
        }       

        if (_playerController.CheckForUseWeaponInput() && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;           
           if(_ammoController.CanUseAmmo())
            {
                OnShoot();                
            }
            else
            {
                //_ammoController.OutOfAmmo();
                           
            }
        }


    }

    public void TryShoot()
    {
        //If gun is empty and player attempts to shoot. trigger empty gun sound
        if (!_ammoController.CanUseAmmo())
        {
            if(bIsActive)
            {
                GetComponent<AudioManager_Archebus>().TriggerEmpty();
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

    IEnumerator OnReload()
    {
        if(bIsActive)
        {
            //Reload Sounds
            GetComponent<AudioManager_Archebus>().TriggerReloadStart();
            StartCoroutine(TriggerReloadEndSound());
            //
        }

        bIsReloading = true;
        gunAnimator.speed = 1 / m_upgradestats.ReloadTime; // adjusts for reload time upgrades
        gunAnimator.SetBool("bIsReloading", true);
        yield return new WaitForSeconds(m_reloadTime);        
        _ammoController.Reload();      
        bIsReloading = false;

        //Play reload and ammo animations      
        gunAnimator.SetBool("bIsReloading", false);          
    }

    void OnShoot()
    {
        //Play Recoil animation
        gunAnimator.SetTrigger("OnRecoil");      
        muzzleFlash.Play();

        if (bIsActive)
        {
            //Gun Shot Sounds
            GetComponent<AudioManager_Archebus>().TriggerShot();
            StartCoroutine(TriggerNewShellSound());
            //
        }

        if (!m_bHasActionUpgrade)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out hitInfo, m_weaponRange))
            {
                //Only damages if asset has "Health" script
                Health target = hitInfo.transform.GetComponent<Health>();
                if (target != null && target.gameObject.tag != "Player")
                {
                    target.TakeDamage(m_damageAmount);
                    reticuleAnimator.SetTrigger("isTargetted");
                }
                else
                {
                    reticuleAnimator.SetTrigger("isTargetted");
                }

                //checks if breakable wall
                DestructibleObject wall = hitInfo.transform.GetComponentInParent<DestructibleObject>();
                if (wall)
                {
                    wall.Break(gameObject.tag);
                }

                /// Evan's Item container call vvv
                ItemContainer container = hitInfo.transform.GetComponentInParent<ItemContainer>();
                if (container)
                {
                    container.Break(gameObject.tag);
                }
                /// Evan's Item container call ^^^
                /// 
                //Force of impact on hit
                if (hitInfo.rigidbody != null)
                {
                    hitInfo.rigidbody.AddForce(-hitInfo.normal * m_hitImpact);
                }

                //Particle effects on hit
                GameObject hitImpact = Instantiate(impactFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(hitImpact, 2.0f);
            }
        }
        else
        {
            UpgradedFire();
        }

        //Using ammo             
        _ammoController.UseAmmo();
    }

    //VR - Plays Animation to focus reticule on targeting
    void OnTarget()
    {
        RaycastHit targetInfo;
        if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out targetInfo, m_weaponRange))
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

    public override void AddUpgrade(WeaponUpgrade upgrade)
    {
        //TODO:: this math sucks, make it better
        m_upgradestats += upgrade;
        m_damageAmount *= upgrade.Damage + 1;
        m_weaponClipSize *= (int)upgrade.ClipSize + 1;
        m_reloadTime *= upgrade.ReloadTime + 1;
        m_fireRate *= upgrade.FireRate + 1;
        m_hitImpact *= upgrade.ImpactForce + 1;
        m_weaponRange *= upgrade.Range + 1;

        if (upgrade.HasAction) m_bHasActionUpgrade = true;

        m_currentupgrades.Add(upgrade.Type);
    }

    //todo: get this figured out
    //public void RemoveUpgrade(WeaponScalars scalars)
    //{
    //    m_scalars -= scalars;
    //    m_weaponClipSize = 6 * m_scalars.ClipSize;
    //    m_reloadTime = 2.0f * m_scalars.ReloadTime;
    //    m_fireRate = 0.8f * m_scalars.FireRate;
    //    m_hitImpact = 50.0f * m_scalars.ImpactForce;
    //    m_weaponRange = 50.0f * m_scalars.Range;
    //}

    void UpgradedFire()
    {
        for (int i = -1; i < 2; i++)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(gunCamera.transform.position, Quaternion.Euler(0, 15f * i, 0) * gunCamera.transform.forward, out hitInfo, m_weaponRange))
            {
                //Only damages if asset has "Health" script
                Health target = hitInfo.transform.GetComponent<Health>();
                if (target != null && target.gameObject.tag != "Player")
                {
                    target.TakeDamage(m_damageAmount);
                    reticuleAnimator.SetTrigger("isTargetted");
                }
                else
                {
                    reticuleAnimator.SetTrigger("isTargetted");
                }

                //checks if breakable wall
                DestructibleObject wall = hitInfo.transform.GetComponentInParent<DestructibleObject>();
                if (wall)
                {
                    wall.Break(gameObject.tag);
                }

                //Force of impact on hit
                if (hitInfo.rigidbody != null)
                {
                    hitInfo.rigidbody.AddForce(-hitInfo.normal * m_hitImpact);
                }

                //Particle effects on hit
                GameObject hitImpact = Instantiate(impactFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(hitImpact, 2.0f);
            }
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "bIsActive", gameObject.scene.name, bIsActive);
        SaveSystem.Save(gameObject.name, "bIsObtained", gameObject.scene.name, bIsObtained);
    }

    public void LoadDataOnSceneEnter()
    {
        bIsActive = SaveSystem.LoadBool(gameObject.name, "bIsActive", gameObject.scene.name);
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", gameObject.scene.name);
    }

    IEnumerator TriggerNewShellSound()
    {
        yield return new WaitForSeconds(0.31f);
        GetComponent<AudioManager_Archebus>().TriggerNewShell();
    }

    IEnumerator TriggerReloadEndSound()
    {
        yield return new WaitForSeconds(1.6f);
        GetComponent<AudioManager_Archebus>().TriggerReloadEnd();
    }
}
