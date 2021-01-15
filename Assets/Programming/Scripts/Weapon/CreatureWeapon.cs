using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CreatureWeapon : Weapon, ISaveable
{
    public Camera _camera;
    public ParticleSystem _spreadEffect;
    GameObject _creatureProjectile;
    public AmmoUI m_ammoUI;

   // private int _bulletClip = 8;

    private void Awake()
    {
        base.Awake();
        _creatureProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Creature Projectile");

        m_weaponClipSize = 8;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        //Initializing AmmoCount and UI
        m_currentAmmoCount = m_weaponClipSize;
        m_overallAmmoCount = m_currentAmmoCount;

        if (m_ammoUI != null)
            m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);


        _camera = FindObjectOfType<Camera>();
        GetComponent<MeshRenderer>().enabled = false;
        bIsActive = false;
        bIsObtained = false;

        m_fireRate = 0.5f;
        m_hitImpact = 10.0f;
    }
    public override void UseTool()
    {
        if (bIsReloading)
        {
            return;
        }

        //Reloads automatically at 0 or if player users reload input "R"
        if (m_currentAmmoCount <= 0 && m_overallAmmoCount >= 1)
        {
            StartCoroutine(OnReload());
            return;
        }
        else if (Input.GetButtonDown("Reload") && m_overallAmmoCount >= 1)
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
                //outOfAmmoAnimator.SetBool("bIsOut", true);
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
        m_currentAmmoCount--;
        if (m_ammoUI != null)
            m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);
        //if (m_currentAmmoCount == 0 && m_overallAmmoCount == 0)
        //{
        //   // outOfAmmoAnimator.SetBool("bIsOut", true);
        //}
    }

    IEnumerator OnReload()
    {
        bIsReloading = true;
       // gunAnimator.SetBool("bIsReloading", true);

        yield return new WaitForSeconds(m_reloadTime);

        while (m_currentAmmoCount < m_weaponClipSize && m_overallAmmoCount > 0)
        {
            m_currentAmmoCount++;
            m_overallAmmoCount--;
        }

        m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);
        bIsReloading = false;

        //Play reload and ammo animations
        //outOfAmmoAnimator.SetBool("bIsOut", false);
        //gunAnimator.SetBool("bIsReloading", false);

    }

    //Function for AmmoPickup class
    public void AmmoPickup(WeaponType type, int numberOfClips)
    {
        m_overallAmmoCount += (m_weaponClipSize * numberOfClips);
        m_ammoUI.SetAmmoText(m_currentAmmoCount, m_overallAmmoCount, m_weaponClipSize);
    }

    private void OnTarget()
    {
        //RaycastHit targetInfo;
        //if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out targetInfo, m_weaponRange))
        //{
        //    //Debug.Log(targetInfo.transform.name);

        //    Health target = targetInfo.transform.GetComponent<Health>();
        //    if (target != null && target.gameObject.tag != "Player")
        //    {
        //        reticuleAnimator.SetBool("isTargetted", true);
        //    }
        //    else
        //    {
        //        reticuleAnimator.SetBool("isTargetted", false);
        //    }
        //}
    }

    public void LoadDataOnSceneEnter()
    {

    }
}
