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

    GameObject playerObject;

    void Start()
    {
        _light = GetComponent<Light>();
        _originalLightColor = _light.color;
        _lightColor = _originalLightColor;
        _originalIntensity = _light.intensity;
        _intensity = _originalIntensity; 
        _playerController = FindObjectOfType<ALTPlayerController>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Orbit()
    {
        _light.transform.eulerAngles += new Vector3( 0.0f, _OrbitSpeed * Time.deltaTime,0.0f);
    }
    void Pulse()
    {


        float intensityIncrement = Mathf.Sin(Time.time * _PulseSpeed);
        _lightColor = new Color(_originalLightColor.r + (intensityIncrement * 0.2f * _Brightness), _originalLightColor.g + (intensityIncrement* 0.15f * _Brightness) , _originalLightColor.b + (intensityIncrement* 0.1f * _Brightness) );
        _lightColor.r += playerObject.transform.position.x * -0.0004f;
        _lightColor.b += playerObject.transform.position.z * -0.0004f;
        _light.color = _lightColor;

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController != null && playerObject != null)
        {
            if (!_playerController.GetDarknessVolume())
            { Pulse(); }
            
            Orbit();
        }
        else
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            _playerController = FindObjectOfType<ALTPlayerController>();
        }
    }
}
