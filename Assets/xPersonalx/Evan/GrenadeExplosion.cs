using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{

    public GameObject Shockwave;
    Light ExplosionLight;
    float Lifetime;

    // Start is called before the first frame update
    void Awake()
    {
        Lifetime = 4.0f;
        ExplosionLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Lifetime -= Time.deltaTime;
        ExplosionLight.intensity -= 100.0f;
        if (Shockwave.transform.localScale.x > 0.0f)
        {
            Shockwave.transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f);
        }
        else 
        {
            Shockwave.transform.localScale = new Vector3();
        }
        Shockwave.transform.Rotate(new Vector3(1.0f, 2.0f, 3.0f));
        if (Lifetime < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
