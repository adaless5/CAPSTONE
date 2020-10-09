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

    float _lightVal = 1.0f;

    [SerializeField] float _lightChangeSpeed = 10f;

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
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _directionalLightState = DirectionalLightState.Decrementing;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _directionalLightState = DirectionalLightState.Incrementing;
        }
    }

    void DecrementLight()
    {
        if(_lightVal >= 0f)
        {
            _lightVal -= Time.deltaTime /*/ _lightChangeSpeed*/;
            _directionalLight.color = new Vector4(_lightVal, _lightVal, _lightVal, 1);
        }
        else
        {
            _lightVal = 0.0f;
            _directionalLightState = DirectionalLightState.Set;
        }
        
    }

    void IncrementLight()
    {
        if (_lightVal <= 1f)
        {
            _lightVal += Time.deltaTime/* / _lightChangeSpeed*/;
            _directionalLight.color = new Vector4(_lightVal, _lightVal, _lightVal, 1);
        }
        else
        {
            _lightVal = 1.0f;
            _directionalLightState = DirectionalLightState.Set;
        }
    }
}
