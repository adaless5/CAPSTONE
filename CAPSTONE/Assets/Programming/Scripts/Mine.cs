﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float FuzeTimer = 3f;
    public float BlastRadius = 10f;
    public float ExplosionForce = 2000f;

    public GameObject explosionParticleEffect;

    float m_Timer;
    bool m_bIsBlownUp;
    // Start is called before the first frame update
    void Start()
    {
        m_Timer = FuzeTimer;
        m_bIsBlownUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
        
        if (!m_bIsBlownUp)
        {
            if (m_Timer <= 0.0f)
            {
                m_bIsBlownUp = true;
                Explode();
            }
        }
    }

    void Explode()
    {
        //playes particle effect
        if (explosionParticleEffect)
        {
            Instantiate(explosionParticleEffect, transform.position, transform.rotation);
        }

        //applies forces to all nearby gameobjects
        Collider[] hits = Physics.OverlapSphere(transform.position, BlastRadius);



        foreach (Collider obj in hits)
        {
            if(obj.GetComponent<BreakableWall>())
            {
                obj.GetComponent<BreakableWall>().Blowdedup();
            }

            if (obj.GetComponent<Rigidbody>())
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, BlastRadius);
            }
        }

        //deletes self from world
        Destroy(gameObject);
    }
}
