using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{

    public GameObject ImplosionWaveObject;
    public GameObject[] ShockwaveObjects;
    Light ExplosionLight;
    float Lifetime;

    // Start is called before the first frame update
    void Awake()
    {
        Lifetime = 4.0f;
        ExplosionLight = GetComponent<Light>();
        for(int i = 0; i < ShockwaveObjects.Length;i++)
        {
            ShockwaveObjects[i].transform.localEulerAngles = new Vector3( Random.Range(-90.0f,90.0f), Random.Range(-90.0f, 90.0f),  0.0f);
            if(i>0)
            {
                ShockwaveObjects[i].transform.localScale *= (float)(i + 1);
            }
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
        {Destroy(gameObject);}


    }
    void Implosion()
    {
        if (ImplosionWaveObject.transform.localScale.x > 0.0f)
        {
            ImplosionWaveObject.transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f);
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
                shockwave.transform.localScale *= 1.5f;

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
                    Destroy(shockwave);
                }
            }

        }
    }
}
