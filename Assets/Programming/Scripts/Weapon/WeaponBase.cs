using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class WeaponBase : Weapon, ISaveable
{
    //Not being used yet, will be implemented with Ammo Pickups - VR
    public enum AmmoTypes
    {
        Default,
        Creature,
        Explosive,
    }

    [Header("UI Elements - ParticleFX and Reticule")]
    public ParticleSystem muzzleFlash;
    public GameObject impactFX;
    public Animator reticuleAnimator;
    public Animator outOfAmmoAnimator;
    [SerializeField]
    Animator gunAnimator;

    [Header("Camera Settings")]
    public Camera gunCamera;
    
    float m_timeElapsed;
    float m_lerpDuration = 3f;

    void Awake()
    {
        base.Awake();
        //EventBroker.OnAmmoPickup += AmmoPickup;
        LoadDataOnSceneEnter();

        //TODO: Readd Save implementation
        //SaveSystem.SaveEvent += SaveDataOnSceneChange;
        //Debug.Log(m_scalars.Damage);
        m_weaponClipSize = 6 * (int)m_scalars.ClipSize;
        m_reloadTime = 2.0f * m_scalars.ReloadTime;
        m_fireRate = 0.8f * m_scalars.FireRate; //Default Gun shoots every 1.25 seconds, can be adjusted in editor - VR
        m_hitImpact = 50.0f * m_scalars.ImpactForce;
        m_weaponRange = 50.0f * m_scalars.Range;
        m_fireStart = 0.0f;       
        outOfAmmoAnimator = FindObjectOfType<AmmoUI>().GetComponent<Animator>();
        gunAnimator = GetComponent<Animator>();

        //Initializing AmmoCount and UI
        //m_currentAmmoCount = m_weaponClipSize;
        //m_overallAmmoCount = m_currentAmmoCount;
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
                outOfAmmoAnimator.SetBool("bIsOut", false);
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
        //if (m_currentAmmoCount <= 0 && m_overallAmmoCount >= 1)
        if(_ammoController.NeedsReload() || (Input.GetButtonDown("Reload") && _ammoController.CanReload()))
        {
            StartCoroutine(OnReload());
            return;
        }       

        if (_playerController.CheckForUseWeaponInput() && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
           // if (m_currentAmmoCount > 0)
           if(_ammoController.CanUseAmmo())
            {
                OnShoot();                
            }
            else
            {
                outOfAmmoAnimator.SetBool("bIsOut", true);               
            }
        }
    }

    IEnumerator OnReload()
    {
        bIsReloading = true;
        gunAnimator.speed = 1 / m_scalars.ReloadTime; // adjusts for reload time upgrades
        gunAnimator.SetBool("bIsReloading", true);
        yield return new WaitForSeconds(m_reloadTime);

        //while (m_currentAmmoCount < m_weaponClipSize && m_overallAmmoCount > 0)
        //{
        //    m_currentAmmoCount++;
        //    m_overallAmmoCount--;
        //}
        _ammoController.Reload();
      
        bIsReloading = false;

        //Play reload and ammo animations
        outOfAmmoAnimator.SetBool("bIsOut", false);
        gunAnimator.SetBool("bIsReloading", false);          
    }

    //Function for AmmoPickup class, currently adding ammo to whatever weapon is active
    //public void AmmoPickup(WeaponType type, int numberOfClips)
    //{
    //    //if (type == WeaponType.BaseWeapon)
    //    if(bIsActive)
    //        m_overallAmmoCount += (m_weaponClipSize * numberOfClips);        
    //}


    void OnShoot()
    {
        //Play Recoil animation
        gunAnimator.SetTrigger("OnRecoil");      
        muzzleFlash.Play();

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
        //m_currentAmmoCount--;      
        _ammoController.UseAmmo();
        //if (m_currentAmmoCount == 0 && m_overallAmmoCount == 0)
        if(_ammoController.OutOfAmmo())
        {
            outOfAmmoAnimator.SetBool("bIsOut", true);
        }        
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

    public override void AddUpgrade(WeaponScalars scalars)
    {
        m_scalars += scalars;
        m_damageAmount *= m_scalars.Damage;
        m_weaponClipSize *= (int)m_scalars.ClipSize;
        m_reloadTime *= m_scalars.ReloadTime;
        m_fireRate *= m_scalars.FireRate;
        m_hitImpact *= m_scalars.ImpactForce;
        m_weaponRange *= m_scalars.Range;
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

    public override void SetHasAction(bool hasaction)
    {
        m_bHasActionUpgrade = hasaction;
    }

    void UpgradedFire()
    {
        Debug.Log("Upgraded fire");
        for (int i = -1; i < 2; i++)
        {
            Debug.Log(i.ToString());
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
}
