using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : Weapon, ISaveable
{
    [Header("Damage Settings")]
    public float m_damageAmount = 10.0f;

    [Header("Weapon Settings")]
    public int m_ammoAmount = 6; //not being used currently
    public float m_fireRate = 10.0f;
    public float m_hitImpact = 50.0f;
    public float m_weaponRange = 50.0f;

    private float m_fireStart = 0.0f;

    [Header("UI Elements - ParticleFX and Reticule")]
    public ParticleSystem muzzleFlash;
    public GameObject impactFX;
    public Animator reticuleAnimator;   
    

    [Header("Camera Settings")]
    public Camera gunCamera;

    public ALTPlayerController _playercontroller;


    public override void Start()
    {

        gunCamera = GameObject.FindObjectOfType<Camera>();
        GetComponent<MeshRenderer>().enabled = true;
        bIsActive = true;
        bIsObtained = true;
    }

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    // Update is called once per frame
    public override void Update()
    {

        if (bIsActive && _playercontroller.m_ControllerState == ALTPlayerController.ControllerState.Play)
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

    public override void UseTool()
    {
        if (Input.GetButton("Fire1") && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            OnShoot();
        }
    }

    void OnShoot()
    {
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
