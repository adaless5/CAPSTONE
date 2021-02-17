using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    float m_FuzeTimer = 3f;
    float m_BlastRadius = 10f;
    float m_ExplosionForce = 2000f;
    float m_Damage = 50;
    bool m_bHasAction = false;

    public GameObject explosionParticleEffect;

    float m_Timer;
    bool m_bIsBlownUp;

    public void InitMine(float fuzetime, float blastradius, float explosionforce, float damage, bool hasaction)
    {
        m_FuzeTimer = fuzetime;
        m_BlastRadius = blastradius;
        m_ExplosionForce = explosionforce;
        m_Damage = damage;
        m_bHasAction = hasaction;

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

        /// Evan's Item container call vvv
        foreach (Collider obj in hits)
        {
            Debug.Log(obj);
            if (obj.GetComponentInParent<ItemContainer>())
            {
                obj.GetComponentInParent<ItemContainer>().Break(gameObject.tag);
            }
        }
        /// Evan's Item container call ^^^


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

        if(m_bHasAction)
        {
            for(int i = 0; i < 4; i++)
            {
                GameObject mine = Instantiate(gameObject, transform.position, transform.rotation);
                if(mine)
                {
                    mine.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                    switch (i)
                    {
                        case 0:
                            mine.GetComponent<Rigidbody>().AddForce(new Vector3(0, 200f, 500f));
                            break;
                        case 1:
                            mine.GetComponent<Rigidbody>().AddForce(new Vector3(0, 200f, -500f));
                            break;
                        case 2:
                            mine.GetComponent<Rigidbody>().AddForce(new Vector3(500f, 200f, 0));
                            break;
                        case 3:
                            mine.GetComponent<Rigidbody>().AddForce(new Vector3(-500f, 200f, 0));
                            break;
                    }
                    mine.GetComponent<Mine>().InitMine(m_FuzeTimer, m_BlastRadius * 0.5f, m_ExplosionForce * 0.5f, m_Damage * 0.5f, false);
                }
            }
        }

        //deletes self from world
        Destroy(gameObject);
    }

    
}
