using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessTrigger : MonoBehaviour
{
    enum DirectionalLightState
    {
        Set, 
        Decrementing,
        Incrementing,
    }

    DirectionalLightState _directionalLightState;
    Light _directionalLight;

    Vector4 _lightVals;
    Vector4 _targetLightVals = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    ALTPlayerController _playerController;

    private void Awake()
    {
        
    }

    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;

        Light[] lightarray = FindObjectsOfType<Light>();

        foreach(Light light in lightarray)
        {
            if (light.gameObject.tag == "Dir Light")
            {
                _directionalLight = light;

                break;
            }
        }
    }

    void Update()
    {
        //The problem here is that the Darkness trigger spawns in before the player so if I init this on awake it returns a null
        //After release 1 this will probably be a good candidate for Leo's event broker. 
        if(_playerController == null)
        {
            _playerController = FindObjectOfType<ALTPlayerController>();
        }

        switch (_directionalLightState)
        {
            case DirectionalLightState.Set:
                break;

            case DirectionalLightState.Decrementing:
                DecrementLight();
                break;

            case DirectionalLightState.Incrementing:
                IncrementLight();
                break;
        }

        if (_playerController != null)
        {
            if (_playerController.GetThermalView())
            {
                _targetLightVals = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            }
            else if (_playerController.GetThermalView() == false)
            {
                _targetLightVals = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _directionalLightState = DirectionalLightState.Decrementing;

            _playerController.SetDarknessVolume(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _directionalLightState = DirectionalLightState.Incrementing;

            _playerController.SetDarknessVolume(false);
        }
    }

    void DecrementLight()
    {
        _lightVals = _directionalLight.color;

        if(_lightVals.x >= 0f && _lightVals.y >= 0f && _lightVals.z >= 0f)
        {
            _lightVals.x -= Time.deltaTime;
            _lightVals.x = Mathf.Clamp(_lightVals.x, 0.0f, 1.0f);

            _lightVals.y -= Time.deltaTime;
            _lightVals.y = Mathf.Clamp(_lightVals.y, 0.0f, 1.0f);

            _lightVals.z -= Time.deltaTime;
            _lightVals.z = Mathf.Clamp(_lightVals.z, 0.0f, 1.0f);

            _directionalLight.color = new Vector4(_lightVals.x, _lightVals.y, _lightVals.z, 1);


        }
        else
        {
            _lightVals.x = 0.0f;
            _lightVals.y = 0.0f;
            _lightVals.z = 0.0f;
            _directionalLightState = DirectionalLightState.Set;
        }
    }

    void IncrementLight()
    {
        _lightVals = _directionalLight.color;

        if (_lightVals.x <= _targetLightVals.x && _lightVals.y <= _targetLightVals.y && _lightVals.z <= _targetLightVals.z)
        {
            _lightVals.x += Time.deltaTime;
            _lightVals.x = Mathf.Clamp(_lightVals.x, 0.0f, _targetLightVals.x);

            _lightVals.y += Time.deltaTime;
            _lightVals.y = Mathf.Clamp(_lightVals.y, 0.0f, _targetLightVals.y);

            _lightVals.z += Time.deltaTime;
            _lightVals.z = Mathf.Clamp(_lightVals.z, 0.0f, _targetLightVals.z);

            _directionalLight.color = new Vector4(_lightVals.x, _lightVals.y, _lightVals.z, 1.0f);
        }
        else
        {
            _lightVals.x = _targetLightVals.x;
            _lightVals.y = _targetLightVals.y;
            _lightVals.z = _targetLightVals.z;
            _directionalLightState = DirectionalLightState.Set;
        }
    }
}
