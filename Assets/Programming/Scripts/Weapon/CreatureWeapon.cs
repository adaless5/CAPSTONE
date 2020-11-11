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

    private int _bulletClip = 8;

    public void Awake()
    {
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
        m_hitImpact = 5.0f;
    }
    public override void UseTool()
    {
        if (Input.GetButton("Fire1") && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            OnShoot();
        }
    }
    public override void Update()
    {

        if (bIsActive)
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



    void OnShoot()
    {



        for (int i = 0; i < _bulletClip; i++)
        {
            Vector3 bulletDeviation = UnityEngine.Random.insideUnitCircle * 300.0f;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * 200.0f + bulletDeviation);
            Vector3 finalFowardVector = transform.rotation * rot * Vector3.forward;
            finalFowardVector += transform.position;

            //GameObject creatureProjectile = Instantiate(_creatureProjectile, finalFowardVector, Quaternion.identity);
            GameObject creatureProjectile = ObjectPool.Instance.SpawnFromPool("Creature", finalFowardVector, Quaternion.identity);
            //Vector3 randomSize = new Vector3(Random)
            //creatureProjectile.transform.localScale = ;
            creatureProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * m_hitImpact, ForceMode.Impulse);



        }

    }

    private void OnTarget()
    {

    }

    public void SaveDataOnSceneChange()
    {

    }

    public void LoadDataOnSceneEnter()
    {

    }
}
