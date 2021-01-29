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
   
    private void Awake()
    {
        base.Awake();      
        _creatureProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Creature Projectile");     
        m_weaponClipSize = 8;
        m_reloadTime = 0.5f;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        //Initializing Ammo Controller
        _ammoController = FindObjectOfType<AmmoUI>().GetComponent<AmmoController>();
        _ammoController.InitializeAmmo(AmmoController.AmmoTypes.Creature, m_weaponClipSize, m_weaponClipSize);       
     
        _camera = FindObjectOfType<Camera>();
        GetComponent<MeshRenderer>().enabled = false;
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
        if(_ammoController.NeedsReload() || (Input.GetButtonDown("Reload") && _ammoController.CanReload()))
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
        }
    }

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

    void OnShoot()
    {
        for (int i = 0; i < m_weaponClipSize; i++)
        {
            Vector3 bulletDeviation = UnityEngine.Random.insideUnitCircle * 300.0f;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * 200.0f + bulletDeviation);
            Vector3 finalFowardVector = transform.rotation * rot * Vector3.forward;
            finalFowardVector += transform.position;

            //GameObject creatureProjectile = Instantiate(_creatureProjectile, finalFowardVector, Quaternion.identity);
            if (ObjectPool.Instance != null)
            {
                //  if (ObjectPool.Instance._poolDictionary.ContainsKey("Creature"))
                {
                    GameObject creatureProjectile = ObjectPool.Instance.SpawnFromPool("Creature", finalFowardVector, Quaternion.identity);
                    float randomfloat = UnityEngine.Random.Range(0.1f, 0.5f);
                    Vector3 randomSize = new Vector3(randomfloat, randomfloat, randomfloat);
                    creatureProjectile.transform.localScale = randomSize;
                    creatureProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * m_hitImpact, ForceMode.Impulse);
                    creatureProjectile.GetComponent<CreatureProjectile>().InitCreatureProjectile(m_maxDamageTime, m_projectileLifeTime, m_damageAmount, m_bHasActionUpgrade);
                }
                //  else
                {
                    //    Debug.LogError("doesnt contain creature key");
                }
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
