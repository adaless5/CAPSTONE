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
    Animator reloadAnimator;

    [Header("Camera Settings")]
    public Camera gunCamera;
    public AmmoUI m_ammoUI;

    Vector3 m_WeaponRecoilLocalPosition;
    Vector3 m_AccumlatedRecoil;
    Vector3 m_OriginalGunPos;

    float m_timeElapsed;
    float m_lerpDuration = 3f;

    void Awake()
    {
        base.Awake();
        EventBroker.OnAmmoPickup += AmmoPickup;
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
        m_weaponClipSize = 6;
        m_reloadTime = 2.0f;
        m_fireRate = 0.8f; //Default Gun shoots every 1.25 seconds, can be adjusted in editor - VR
        m_hitImpact = 50.0f;
        m_weaponRange = 50.0f;
        m_fireStart = 0.0f;
        m_recoilForce = .50f;
        m_maxRecoilDistance = 0.5f;
        m_recoilInitialSpeed = 25f;
        m_recoilReadjustSpeed = 10f;
        outOfAmmoAnimator = FindObjectOfType<AmmoUI>().GetComponent<Animator>();
        reloadAnimator = GetComponent<Animator>();

        m_OriginalGunPos = gameObject.transform.localPosition;
        m_WeaponRecoilLocalPosition = m_OriginalGunPos;
        m_AccumlatedRecoil = m_OriginalGunPos;
    }

    public override void Start()
    {
        //Initializing AmmoCount and UI
        m_currentAmmoCount = m_weaponClipSize;
        m_overallAmmoCount = m_currentAmmoCount;

        if (m_ammoUI != null)
            m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);

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

    private void LateUpdate()
    {
        WeaponRecoilUpdate();
        gameObject.transform.localPosition = m_WeaponRecoilLocalPosition;
    }

    //Updates weapon recoil animation - VR
    //TODO: Not currently using lerping speed for recoil to match fire rate as weapon won't return to original position
    void WeaponRecoilUpdate()
    {
        //Going from local position to recoil
        if (m_WeaponRecoilLocalPosition.z >= m_AccumlatedRecoil.z *0.99f)
        {
            m_WeaponRecoilLocalPosition.z = Mathf.Lerp(m_WeaponRecoilLocalPosition.z, m_AccumlatedRecoil.z, m_recoilInitialSpeed * Time.deltaTime);
            //m_WeaponRecoilLocalPosition.z = m_AccumlatedRecoil.z;
        }
        else //Going from guns new position after recoil back to original
        {             
            m_WeaponRecoilLocalPosition.z = Mathf.Lerp(m_WeaponRecoilLocalPosition.z, m_OriginalGunPos.z, 1f);
            m_AccumlatedRecoil.z = m_WeaponRecoilLocalPosition.z;           
        }
    }

    public override void UseTool()
    {
        if (bIsReloading)
        {
            return;
        }

        //Reloads automatically at 0 or if player users reload input "R"
        if (m_currentAmmoCount <= 0 && m_overallAmmoCount >= 6 || Input.GetButtonDown("Reload"))
        {
            StartCoroutine(OnReload());
            return;
        }

        if (_playerController.CheckForUseWeaponInput() && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            if (m_currentAmmoCount > 0)
            {
                OnShoot();
            }
            else
            {
                outOfAmmoAnimator.SetBool("bIsOut", true);
                Debug.Log("Out of Ammo");
            }
        }
    }

    IEnumerator OnReload()
    {
        bIsReloading = true;
        reloadAnimator.SetBool("bIsReloading", true);        

        Debug.Log("Reloading ammo");

        yield return new WaitForSeconds(m_reloadTime);

        while (m_currentAmmoCount < m_weaponClipSize && m_overallAmmoCount > 0)
        {
            m_currentAmmoCount++;
            m_overallAmmoCount--;
        }

        m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);
        bIsReloading = false;

        outOfAmmoAnimator.SetBool("bIsOut", false);
        reloadAnimator.SetBool("bIsReloading", false);       
        Debug.Log("Reload Complete");
    }

    //Function for AmmoPickup class
    public void AmmoPickup(WeaponType type, int numberOfClips)
    {
        m_overallAmmoCount += (m_weaponClipSize * numberOfClips);
        m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);
    }


    void OnShoot()
    {

        //Weapon Recoil amount 
        m_AccumlatedRecoil.z += Vector3.back.z * m_recoilForce;       
        m_AccumlatedRecoil = Vector3.ClampMagnitude(m_AccumlatedRecoil, m_maxRecoilDistance);


        muzzleFlash.Play();

        RaycastHit hitInfo;
        if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out hitInfo, m_weaponRange))
        {
            //Debug.Log(hitInfo.transform.name);

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

        //Using ammo
        m_currentAmmoCount--;
        if (m_ammoUI != null)
            m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);
        if (m_currentAmmoCount == 0 && m_overallAmmoCount == 0)
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
            //Debug.Log(targetInfo.transform.name);

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
