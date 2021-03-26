using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitEffects : MonoBehaviour
{
    public ParticleSystem[] HitParticles;
    public ParticleSystem DeathParticles;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < HitParticles.Length; i++)
        {
            HitParticles[i].Stop();
        }
        DeathParticles.Stop();
    }
    public void Hit()
    {
        for(int i = 0; i <HitParticles.Length; i++)
        {
            if(!HitParticles[i].isPlaying)
            {
                HitParticles[i].Play();
                return;
            }
        }
    }
    public void Death()
    {
        Instantiate(DeathParticles, transform.position, transform.rotation);
        DeathParticles.Play();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
