using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingTechLight : MonoBehaviour
{
    // Start is called before the first frame update

    public Light Light1;
    public Light Light2;
    public Light Light3;

    public float FlickerLength;
    public float FlickerTimer;
    public bool Flickering;



    public float OriginalIntensity;
    void Start()
    {

        FlickerLength = Random.Range(0.5f, 2.0f);
        FlickerTimer = Random.Range(2.0f, 20.0f);
    }
    void Flicker()
    {
        if (FlickerLength > 0.0f)
        {
            FlickerLength -= Time.deltaTime;
            float randomIntensity = Random.Range(0.0f, OriginalIntensity);
            Light1.intensity = randomIntensity;
            Light2.intensity = randomIntensity;
            Light3.intensity = randomIntensity;
        }
        else
        {
            FlickerLength = Random.Range(0.5f, 2.0f);
            FlickerTimer = Random.Range(5.0f, 40.0f);
            Flickering = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (OriginalIntensity < 0.001)
        {
            OriginalIntensity = Light1.intensity;
        }

        if (Flickering)
        {
            Flicker();
        }
        else
        {
            FlickerTimer -= Time.deltaTime;
            if (Light1.intensity < OriginalIntensity)
            {
                Light1.intensity = OriginalIntensity;
                Light2.intensity = OriginalIntensity;
                Light3.intensity = OriginalIntensity;
            }
            if (FlickerTimer < 0.0f)
            {
                Flickering = true;
            }
        }



    }
}
