using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomassDeath : MonoBehaviour
{
    // Start is called before the first frame update
    float dtime = 4.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dtime -= Time.deltaTime;
        transform.localScale *= 0.9f;
        if(dtime<0.0f)
        {
            Destroy(gameObject);
        }
    }
}
