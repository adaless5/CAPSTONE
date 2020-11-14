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


    [Header("Camera Settings")]
    public Camera gunCamera;

    public AmmoUI m_ammoUI;
   

    void Awake()
    {
        base.Awake();
        EventBroker.OnAmmoPickup += AmmoPickup;
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
        m_weaponClipSize = 6;
        m_reloadTime = 2.0f;
        m_fireRate = 10.0f;
        m_hitImpact = 50.0f;
        m_weaponRange = 50.0f;
        m_fireStart = 0.0f;
    }

    public override void Start()
    {
        //Initializing AmmoCount and UI
        m_currentAmmoCount = m_weaponClipSize;
        m_overallAmmoCount = m_currentAmmoCount;

        if (m_ammoUI != null)
            m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount);

        gunCamera = GameObject.FindObjectOfType<Camera>();
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
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
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
        if(bIsReloading)
        {
            return;
        }

        //Currently reloading automatically, can change based on player input at later date
        if (m_currentAmmoCount <= 0 && m_overallAmmoCount >= 6)
        {
            StartCoroutine(OnReload());
            return;
        }

        if (_playerController.CheckForUseWeaponInput() && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            if(m_currentAmmoCount > 0)
            {
                OnShoot();
            }
            else
            {
                Debug.Log("Out of Ammo");
            }
        }
    }

    IEnumerator OnReload()
    {
        bIsReloading = true;
        Debug.Log("Reloading ammo");

        yield return new WaitForSeconds(m_reloadTime);

        m_currentAmmoCount = m_weaponClipSize;
        m_overallAmmoCount -= m_weaponClipSize;
        m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount);
        bIsReloading = false;

        Debug.Log("Reload Complete");
    }

    //Function for AmmoPickup class
    public void AmmoPickup(WeaponType type, int numberOfClips)
    {
        m_overallAmmoCount += (m_weaponClipSize * numberOfClips);
        m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount);
    }


    void OnShoot()
    {
        Debug.Log("Current Ammo: " + m_currentAmmoCount);
        Debug.Log("Overall Ammo: " + m_overallAmmoCount);

        muzzleFlash.Play();

        RaycastHit hitInfo;
        if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out hitInfo, m_weaponRange))
        {
            //Debug.Log(hitInfo.transform.name);

            //Only damages if asset has "Target" script
            Health target = hitInfo.transform.GetComponent<Health>();
            if (target != null)
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
            if(wall)
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

        //Using ammo
        m_currentAmmoCount--;        
        if (m_ammoUI != null)
            m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount);
    }

   //VR - Plays Animation to focus reticule on targeting
    void OnTarget()
    {
        RaycastHit targetInfo;
        if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out targetInfo, m_weaponRange))
        {
            //Debug.Log(targetInfo.transform.name);

       
            Health target = targetInfo.transform.GetComponent<Health>();
            if (target != null)
            {
                reticuleAnimator.SetBool("isTargetted", true);
            }
            else
            {
                reticuleAnimator.SetBool("isTargetted", false);
            }
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "bIsActive", bIsActive);
        SaveSystem.Save(gameObject.name, "bIsObtained", bIsObtained);
    }   

    public void LoadDataOnSceneEnter()
    {
        bIsActive = SaveSystem.LoadBool(gameObject.name, "bIsActive");
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained");
    }
}
