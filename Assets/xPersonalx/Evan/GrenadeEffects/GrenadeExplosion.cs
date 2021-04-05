using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{

    public GameObject ImplosionWaveObject;
    public GameObject[] ShockwaveObjects;
    Light ExplosionLight;
    float Lifetime;
    public bool bDontDie;
    float lightIntensity;

    // Start is called before the first frame update
    void Awake()
    {
        bDontDie = false;
        Lifetime = 0.6f;
        ExplosionLight = GetComponent<Light>();
        lightIntensity = ExplosionLight.intensity;
        for (int i = 0; i < ShockwaveObjects.Length;i++)
        {
            ShockwaveObjects[i].transform.localEulerAngles = new Vector3( Random.Range(-90.0f,90.0f), Random.Range(-90.0f, 90.0f),  0.0f);
            if(i>0)
            {
                ShockwaveObjects[i].transform.localScale *= (float)(i + 1);
            }
        }
    }

    void ResetExplosion()
    {
        ExplosionLight.intensity = lightIntensity;
        Lifetime = 0.6f;
        GameObject.Find("LightningExplosionParticles").GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < ShockwaveObjects.Length; i++)
        {
            ShockwaveObjects[i].transform.localEulerAngles = new Vector3(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), 0.0f);
            if (i > 0)
            {
                ShockwaveObjects[i].transform.localScale = new Vector3((i + 1), (i + 1) * 0.33f, (i + 1));
            }
        }
        ImplosionWaveObject.transform.localScale = new Vector3(10, 10, 10);
        foreach (GameObject shockwave in ShockwaveObjects)
        {
            Material mat = shockwave.GetComponent<MeshRenderer>().material;
            Color newColor = mat.color;
            newColor.a = 1;
            mat.color = newColor;
            shockwave.GetComponent<MeshRenderer>().material = mat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Lifetime -= Time.deltaTime;
        ExplosionLight.intensity -= 100.0f;

        Implosion();
        Shockwave();

        if (Lifetime < 0.0f)
        {
            if(bDontDie)
            {
                ResetExplosion();
            }
            else
            Destroy(gameObject);
        }


    }
    void Implosion()
    {
        if (ImplosionWaveObject.transform.localScale.x > 0.0f)
        {
            ImplosionWaveObject.transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * 0.7f;
        }
        else 
        {
            ImplosionWaveObject.transform.localScale = new Vector3();
        }
        ImplosionWaveObject.transform.Rotate(new Vector3(1.0f, 2.0f, 3.0f));
    }
    void Shockwave()
    {
        if(ShockwaveObjects[0] !=null)
        {

            foreach (GameObject shockwave in ShockwaveObjects)
            {
                shockwave.transform.localScale *= 1.1f;

                Material mat = shockwave.GetComponent<MeshRenderer>().material;
                if (mat.color.a > 0.01f)
                {
                    Color newColor = mat.color;
                    newColor.a -= Time.deltaTime * 4.0f;
                    mat.color = newColor;
                    shockwave.GetComponent<MeshRenderer>().material = mat;
                }
                else
                {
                    Color newColor = mat.color;
                    newColor.a = 0;
                    mat.color = newColor;
                    shockwave.GetComponent<MeshRenderer>().material = mat;
                }
            }

        }
    }
}
