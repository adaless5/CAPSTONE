using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingLight : MonoBehaviour
{
    public float pulseHigh = 1.0f;//The highest intensity of the pulse. 1.0 is the original intensity of the light.
    public float pulseLow = 0.0f;//The Lowest intensity of the pulse. 0.0 is no light, 1.0 is the original intensity of the light.
    public float pulseTime = 1.0f;// the length of time it takes for a full pulse to occur.
    Light _Light;
    float originalIntensity;
    bool UpDown;
    float currentPulseTime;
    // Start is called before the first frame update
    void Awake()
    {
        if (pulseLow > pulseHigh)
        {
            pulseLow = pulseHigh;
        }
        _Light = GetComponent<Light>();
        originalIntensity = _Light.intensity;
    }
    void CountPulse()
    {
        if (UpDown)
        {
            currentPulseTime += Time.deltaTime;
            if (currentPulseTime > pulseTime)
            {
                UpDown = false;
            }
        }
        else
        {
            currentPulseTime -= Time.deltaTime;
            if (currentPulseTime < 0.0f)
            {
                UpDown = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CountPulse();
        _Light.intensity = Mathf.Lerp(pulseLow, pulseHigh, Mathf.Sin(currentPulseTime / pulseTime));
    }
}
