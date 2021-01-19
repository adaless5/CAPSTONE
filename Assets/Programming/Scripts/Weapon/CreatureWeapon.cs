﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CreatureWeapon : Weapon, ISaveable
{
    public Camera _camera;
    public ParticleSystem _spreadEffect;
    GameObject _creatureProjectile;

    private int _bulletClip = 8;

    private void Awake()
    {
        base.Awake();
        _creatureProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Creature Projectile");

    }

    // Start is called before the first frame update
    public override void Start()
    {

        _camera = FindObjectOfType<Camera>();
        GetComponent<MeshRenderer>().enabled = false;
        bIsActive = false;
        bIsObtained = false;

        m_fireRate = 0.5f;
        m_hitImpact = 10.0f;
        m_damageAmount = 5.0f;
        m_maxDamageTime = 1f;
        m_projectileLifeTime = 6.0f;
    }

    
    public override void UseTool()
    {
        if (_playerController.CheckForUseWeaponInput() && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            OnShoot();
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
        for (int i = 0; i < _bulletClip; i++)
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
                    creatureProjectile.GetComponent<CreatureProjectile>().InitCreatureProjectile(m_maxDamageTime, m_projectileLifeTime, m_damageAmount);
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
    }

    public override void AddUpgrade(WeaponScalars scalars)
    {
        m_scalars += scalars;
        m_damageAmount *= m_scalars.Damage;
        m_weaponClipSize *= m_scalars.ClipSize;
        m_reloadTime *= m_scalars.ReloadTime;
        m_fireRate *= m_scalars.FireRate;
        m_hitImpact *= m_scalars.ImpactForce;
        m_weaponRange *= m_scalars.Range;

        Debug.Log(m_scalars.Damage);
    }

    private void OnTarget()
    {

    }

    public void LoadDataOnSceneEnter()
    {

    }
}
