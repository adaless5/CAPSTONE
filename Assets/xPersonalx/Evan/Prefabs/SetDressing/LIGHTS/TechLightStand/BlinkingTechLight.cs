using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingTechLight : MonoBehaviour
{
    // Start is called before the first frame update

    public Light[] Lights;

    public float FlickerLength;
    public float FlickerTimer;
    public bool Flickering;

    float[] glowModifier;
    bool[] bGlowModifier;


    public float OriginalIntensity;
    void Start()
    {
        glowModifier = new float[Lights.Length];
        bGlowModifier = new bool[Lights.Length];
        for (int i = 0; i < Lights.Length; i++)
        {
            glowModifier[i] = (1.0f / Lights.Length) * i;
        }
        OriginalIntensity = Lights[0].intensity;
        FlickerLength = Random.Range(0.5f, 2.0f);
        FlickerTimer = Random.Range(2.0f, 30.0f);
    }
    void Glow()
    {
        for (int i = 0; i < Lights.Length; i++)
        {
            Lights[i].intensity = OriginalIntensity * glowModifier[i];
        }
        GlowModifierOscilate();
    }
    void GlowModifierOscilate()
    {
        for (int i = 0; i < Lights.Length; i++)
        {
            if (bGlowModifier[i])
            {
                glowModifier[i] += Time.deltaTime * 0.5f;
                if (glowModifier[i] > 1.0f)
                {
                    bGlowModifier[i] = false;
                }
            }
            else
            {
                glowModifier[i] -= Time.deltaTime * 0.2f;
                if (glowModifier[i] < 0.5f)
                {
                    bGlowModifier[i] = true;
                }
            }
        }
    }
    void Flicker()
    {
        if (FlickerLength > 0.0f)
        {
            FlickerLength -= Time.deltaTime;
            float randomIntensity = Random.Range(0.0f, OriginalIntensity);
            for (int i = 0; i < Lights.Length; i++)
            {
                Lights[i].intensity = randomIntensity;
            }
        }
        else
        {
            FlickerLength = Random.Range(0.5f, 2.0f);
            FlickerTimer = Random.Range(2.0f, 30.0f);
            Flickering = false;
            for (int i = 0; i < Lights.Length; i++)
            {
                Lights[i].intensity = OriginalIntensity;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Flickering)
        {
            Flicker();
        }
        else
        {
            Glow();
            FlickerTimer -= Time.deltaTime;
            if (FlickerTimer < 0.0f)
            {
                Flickering = true;
            }
        }
        


    }
}
