using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenedClosedEffects : MonoBehaviour
{
    // Start is called before the first frame update

    public ParticleSystem _openedParticles;
    public ParticleSystem _closedParticles;

    public ParticleSystem _openingParticles;
    public ParticleSystem _closingParticles;

    public Light _OpenedLight;
    public Light _ClosedLight;

    public float _fadeSpeed;

    float _originalClosedLightIntensity;
    float _originalOpenedLightIntensity;

    private void Start()
    {
        _originalClosedLightIntensity = _OpenedLight.intensity;
        _originalOpenedLightIntensity = _ClosedLight.intensity;
    }
    void LightFade(bool inOut, Light light, float TargetIntensity)
    {
        if (inOut)
        {
            if (light.intensity < TargetIntensity)
            { light.intensity += Time.deltaTime * _fadeSpeed; }
            else
            { light.intensity = TargetIntensity; }
        }
        else
        {
            if (light.intensity > TargetIntensity)
            { light.intensity -= Time.deltaTime * _fadeSpeed; }
            else
            { light.intensity = TargetIntensity; }
        }
    }
    public void Opening()
    {
        if (_openingParticles)
            _openingParticles.Play();

        if (_ClosedLight)
        { 
            LightFade(false, _ClosedLight, 0.0f); 
        }
        if (_OpenedLight)
        {
            LightFade(true, _OpenedLight, _originalOpenedLightIntensity);
        }
    }
    public void Opened()
    {
        if (_openedParticles)
            _openedParticles.Play();
        if (_openingParticles)
            _openingParticles.Stop();
        if (_closedParticles)
            _closedParticles.Stop();
        if (_closingParticles)
            _closingParticles.Stop();
        if (_OpenedLight)
        {
            //LightFade(false, _OpenedLight, 0.0f);
            _OpenedLight.intensity = _originalOpenedLightIntensity;
        }
        if (_ClosedLight)
        {
            //LightFade(true, _ClosedLight, _originalClosedLightIntensity);
            _ClosedLight.intensity = 0.0f;
        }
    }

    public void Closing()
    {
        if (_closingParticles)
            _closingParticles.Play();
        if (_OpenedLight)
        {
            LightFade(false, _OpenedLight, 0.0f);
        }
        if (_ClosedLight)
        {
            LightFade(true, _ClosedLight, _originalClosedLightIntensity);
        }

    }

    public void Closed()
    {
        if (_closedParticles)
            _closedParticles.Play();
        if (_closingParticles)
            _closingParticles.Stop();
        if (_openedParticles)
            _openedParticles.Stop();
        if (_openingParticles)
            _openingParticles.Stop();
        if (_OpenedLight)
        {
            //LightFade(false, _OpenedLight, 0.0f);
            _OpenedLight.intensity = 0.0f;
        }
        if (_ClosedLight)
        {
            //LightFade(true, _ClosedLight, _originalClosedLightIntensity);
            _ClosedLight.intensity = _originalClosedLightIntensity;
        }

    }

}
