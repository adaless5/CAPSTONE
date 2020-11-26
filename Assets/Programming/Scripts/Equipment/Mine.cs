using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float FuzeTimer = 3f;
    public float BlastRadius = 10f;
    public float ExplosionForce = 2000f;
    public int Damage = 50;

    public GameObject explosionParticleEffect;

    float m_Timer;
    bool m_bIsBlownUp;
    // Start is called before the first frame update
    void Start()
    {
        m_Timer = FuzeTimer;
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
        Collider[] hits = Physics.OverlapSphere(transform.position, BlastRadius);

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
                obj.transform.GetComponent<ALTPlayerController>().CallOnTakeDamage(Damage);
            }
            else
            {
                Health target = obj.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(Damage);

                }
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
