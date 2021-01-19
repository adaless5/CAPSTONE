using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    float m_FuzeTimer = 3f;
    float m_BlastRadius = 10f;
    float m_ExplosionForce = 2000f;
    float m_Damage = 50;

    public GameObject explosionParticleEffect;

    float m_Timer;
    bool m_bIsBlownUp;

    public void InitMine(float fuzetime, float blastradius, float explosionforce, float damage)
    {
        m_FuzeTimer = fuzetime;
        m_BlastRadius = blastradius;
        m_ExplosionForce = explosionforce;
        m_Damage = damage;


        m_Timer = m_FuzeTimer;
        m_bIsBlownUp = false;

        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(m_Timer);


        //playes particle effect
        if (explosionParticleEffect)
        {
            Instantiate(explosionParticleEffect, transform.position, transform.rotation);
        }


        //checks if its a breakable wall and breaks it
        Collider[] hits = Physics.OverlapSphere(transform.position, m_BlastRadius);

        foreach (Collider obj in hits)
        {
            Debug.Log(obj);
            if (obj.GetComponentInParent<DestructibleObject>())
            {
                obj.GetComponentInParent<DestructibleObject>().Break(gameObject.tag);
            }
        }

        //applies damage and forces to all nearby gameobjects

        foreach (Collider obj in hits)
        {
            //applies damage
            if (obj.transform.GetComponent<ALTPlayerController>())
            {
                obj.transform.GetComponent<ALTPlayerController>().CallOnTakeDamage(m_Damage);
            }
            else
            {
                Health target = obj.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(m_Damage);

                }
            }

            if (obj.GetComponent<Rigidbody>())
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(m_ExplosionForce, transform.position, m_BlastRadius);
            }
        }

        //deletes self from world
        Destroy(gameObject);
    }

    
}
