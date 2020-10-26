using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;



public class CreatureWeapon : Weapon, ISaveable
{
    public Camera _camera;
    public ParticleSystem _spreadEffect;

    private int _bulletClip = 8;

    // Start is called before the first frame update
    public override void Start()
    {

        _camera = FindObjectOfType<Camera>();
        GetComponent<MeshRenderer>().enabled = false;
        bIsActive = false;
        bIsObtained = false;

        m_fireRate = 0.5f;
        m_hitImpact = 10.0f;
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
        //_spreadEffect.Play();
        //RaycastHit hitInfo;
        //if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, m_weaponRange))
        //{
        //    //Debug.Log(hitInfo.transform.name);

        //    //Only damages if asset has "Target" script
        //    Health target = hitInfo.transform.GetComponent<Health>();
        //    if (target != null)
        //    {
        //        target.TakeDamage(m_damageAmount);
        //    }
        //    else
        //    {
        //    }

        //    //checks if breakable wall
        //    DestructibleObject wall = hitInfo.transform.GetComponentInParent<DestructibleObject>();
        //    if (wall)
        //    {
        //        wall.Break(gameObject.tag);
        //    }

        //    //Force of impact on hit
        //    if (hitInfo.rigidbody != null)
        //    {
        //        hitInfo.rigidbody.AddForce(-hitInfo.normal * m_hitImpact);
        //    }

        //    //Particle effects on hit
        //    //GameObject hitImpact = Instantiate(impactFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        //    //Destroy(hitImpact, 2.0f);
        //}

        //Debug.Log("Begin Shooting...");


        for (int i = 0; i < _bulletClip; i++)
        {
            Vector3 bulletDeviation = UnityEngine.Random.insideUnitCircle * 300.0f;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * 200.0f + bulletDeviation);
            Vector3 finalFowardVector = transform.rotation * rot * Vector3.forward;
            finalFowardVector += transform.position;

            ///CreatureProjectile creatureProjectile = Instantiate(CreatureProjectile, finalFowardVector, Quaternion.identity);

            //if (Physics.Raycast(finalFowardVector, transform.forward,out hit, m_weaponRange))
            //{

            //    //Only damages if asset has "Target" script
            //    Health target = hit.transform.GetComponent<Health>();
            //    if (target != null)
            //    {
            //        target.TakeDamage(m_damageAmount);
            //    }
            //    else
            //    {
            //    }
            //    //Force of impact on hit
            //    if (hit.rigidbody != null)
            //    {
            //        hit.rigidbody.AddForce(-hit.normal * m_hitImpact, ForceMode.Impulse);
            //    }

            //}


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
