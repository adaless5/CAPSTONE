using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLight : MonoBehaviour
{
    // Start is called before the first frame update
    Light FuseLight;
    private float FuseTimer;
    private float LightTimer;
    bool LightOn;
    float OriginalIntensity;

    void Awake()
    {
        LightOn = true;
        FuseTimer = 3.0f;
        FuseLight = GetComponent<Light>();
        LightTimer = 1.0f;
        OriginalIntensity = FuseLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        FuseTimer -= Time.deltaTime;
        Flash();

    }
    void Flash()
    {
        if (FuseTimer > 2.0f)
        {
            LightTimer -= Time.deltaTime * 4.0f;
        }
        else if (FuseTimer > 1.0f)
        {

            LightTimer -= Time.deltaTime * 8.0f;
        }
        else
        {

            LightTimer -= Time.deltaTime * 16.0f;
        }
        if(LightTimer <= 0.0f)
        {
            LightOn = !LightOn;
            if(LightOn)
            {
                FuseLight.intensity = OriginalIntensity;
            }
            else
            {
                FuseLight.intensity = 0.0f;
            }

             LightTimer = 1.0f;
        }

    }

}
