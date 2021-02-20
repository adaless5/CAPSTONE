using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGlow : MonoBehaviour
{
    // Start is called before the first frame update
    Light _light;

    Color _originalLightColor;
    Color _lightColor;

    float _originalIntensity;
    float _intensity;

    public float _Brightness = 0.1f;

    public float _PulseSpeed = 0.5f;
    float _pulseTime;

    public float _OrbitSpeed = 0.5f;
    float _orbitTime;

    ALTPlayerController _playerController;


    void Start()
    {
        _light = GetComponent<Light>();
        _originalLightColor = _light.color;
        _lightColor = _originalLightColor;
        _originalIntensity = _light.intensity;
        _intensity = _originalIntensity; 
        _playerController = FindObjectOfType<ALTPlayerController>();
    }

    void Orbit()
    {
        _light.transform.eulerAngles += new Vector3( 0.0f, _OrbitSpeed * Time.deltaTime,0.0f);
    }
    void Pulse()
    {


        float intensityIncrement = Mathf.Sin(Time.time * _PulseSpeed);
        _lightColor = new Color(_originalLightColor.r + (intensityIncrement * 1.0f * _Brightness), _originalLightColor.g + (intensityIncrement* 0.5f * _Brightness) , _originalLightColor.b + (intensityIncrement* 0.3f * _Brightness) );
       
        _light.color = _lightColor;

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController != null)
        {
            if (!_playerController.GetDarknessVolume())
            { Pulse(); }
            
            Orbit();
        }
        else
        {

            _playerController = FindObjectOfType<ALTPlayerController>();
        }
    }
}
